using UnityEngine;
using Zenject;
using UniRx;
using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Games;
using System.Collections;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace Ryocatusn.Photographers
{
    public class Photographer : MonoBehaviour
    {
        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;
        [SerializeField]
        private Camera photographerCamera;
        [SerializeField]
        private Material material;
        [SerializeField]
        private SE noiseSE;

        [Inject]
        private GameManager gameManager;

        private SEPlayer sePlayer;

        private IPhotographerSubject target;

        private void Start()
        {
            ChangeNoiseAlpha(1);
            sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            //ランダムにターゲットを映す
            StartCoroutine(ChangeTargetEnumerator());
            IEnumerator ChangeTargetEnumerator()
            {
                yield return new WaitForSeconds(5);

                //新しく追加されたターゲットのPriorityが高ければ映す
                photographerSubjectManager.SaveSubject
                    .Where(x => IsAllowedToChangeTarget(x))
                    .Subscribe(x => ChangeTarget(x))
                    .AddTo(this);

                while (true)
                {
                    yield return new WaitForSeconds(Random.Range(6, 12));
                    if (photographerSubjectManager.TryFindRandom(out IPhotographerSubject photographerSubject))
                    {
                        ChangeTarget(photographerSubject);
                    }
                }
            }
        }

        private bool IsAllowedToChangeTarget(IPhotographerSubject photographerSubject)
        {
            if (target == null) return true;
            if (photographerSubject.priority > target.priority) return true;
            else return false;
        }
        private void ChangeTarget(IPhotographerSubject photographerSubject)
        {
            MakeNoiseMoment();

            target = photographerSubject;
            photographerCamera.orthographicSize = target.photographerCameraSize;

            target.showOnPhotographerEvent?.OnNext(Unit.Default);

            photographerSubject.gameObject.OnDestroyAsObservable()
                .Subscribe(_ => OnTargetDestroyed())
                .AddTo(this);
        }

        private void MakeNoiseMoment()
        {
            sePlayer.Play(noiseSE);

            DOTween.Sequence()
                .AppendCallback(() => ChangeNoiseAlpha(1))
                .Append(ChangeAlphaTween(0, 3)).SetEase(Ease.OutBounce);
        }
        private void MakeNoise()
        {
            sePlayer.Play(noiseSE);

            DOTween.Sequence()
                .AppendCallback(() => ChangeNoiseAlpha(0))
                .Append(ChangeAlphaTween(1, 1)).SetEase(Ease.Linear);
        }
        private void ChangeNoiseAlpha(float alpha)
        {
            material.SetFloat("_alpha", alpha);
        }
        private Tween ChangeAlphaTween(float endValue, float duration)
        {
            return DOTween.To
                (
                () => material.GetFloat("_alpha"),
                x => ChangeNoiseAlpha(x),
                endValue,
                duration
                );
        }

        private void OnTargetDestroyed()
        {
            target = null;
            MakeNoise();
        }

        private void Update()
        {
            if (target == null) return;

            Vector3 targetPosition = target.GetPosition();
            photographerCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
    }
}
