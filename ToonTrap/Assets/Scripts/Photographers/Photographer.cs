using UnityEngine;
using Zenject;
using UniRx;
using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Games;

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

            photographerSubjectManager.SaveSubject
                .Where(x => IsAllowedToChangeTarget(x))
                .Subscribe(x => ChangeTarget(x))
                .AddTo(this);
        }

        private bool IsAllowedToChangeTarget(IPhotographerSubject photographerSubject)
        {
            if (target == null) return true;
            if (photographerSubject.priority > target.priority) return true;
            else return false;
        }
        private void ChangeTarget(IPhotographerSubject photographerSubject)
        {
            MakeNoise();

            target = photographerSubject;
            photographerCamera.orthographicSize = target.photographerCameraSize;
        }

        private void MakeNoise()
        {
            sePlayer.Play(noiseSE);

            DOTween.Sequence()
                .AppendCallback(() => ChangeNoiseAlpha(1))
                .Append(ChangeAlpha(0)).SetEase(Ease.OutBounce);

            Tween ChangeAlpha(float endValue)
            {
                return DOTween.To
                    (
                    () => material.GetFloat("_alpha"),
                    x => ChangeNoiseAlpha(x),
                    endValue,
                    3
                    );
            }
        }
        private void ChangeNoiseAlpha(float alpha)
        {
            material.SetFloat("_alpha", alpha);
        }

        private void Update()
        {
            if (target == null) return;
            Vector3 targetPosition = target.GetPosition();
            photographerCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
    }
}
