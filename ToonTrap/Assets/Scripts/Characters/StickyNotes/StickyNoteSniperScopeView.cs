using UnityEngine;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using System;
using Ryocatusn.Audio;
using Zenject;
using Ryocatusn.Games;
using System.Collections;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class StickyNoteSniperScopeView : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private SEPlayer sePlayer;

        [SerializeField]
        private SE appearSE;
        [SerializeField]
        private SE warning;

        [SerializeField]
        private ParticleSystem attackEffect1;
        [SerializeField]
        private ParticleSystem attackEffect2;

        [Inject]
        private GameManager gameManager;

        private Subject<Unit> shotSubject = new Subject<Unit>();

        public IObservable<Unit> ShotSubject => shotSubject;

        public void SetUp(AttackableObjectId attackableObjectId, int chaseTime)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            ShotSubject
                .Subscribe(_ => PlayAttackEffects())
                .AddTo(this);

            StartCoroutine(PlayEnumerator());
            IEnumerator PlayEnumerator()
            {
                bool finishAppear = false;
                Appear(() => finishAppear = true);
                yield return new WaitUntil(() => finishAppear);
                yield return new WaitForSeconds(chaseTime);
                bool finishDisappear = false;
                Disappear(() => finishDisappear = true);
                yield return new WaitUntil(() => finishDisappear);
                shotSubject.OnNext(Unit.Default);
            }
        }
        private void OnDestroy()
        {
            shotSubject.Dispose();
        }

        private void Appear(Action finish)
        {
            StartCoroutine(AppearEnumerator());
            IEnumerator AppearEnumerator()
            {
                for (int i = 0; i < 5; i++)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    yield return new WaitForSeconds(1f / 5);
                }
                spriteRenderer.enabled = true;
                finish();
            }
        }

        private void Disappear(Action finish)
        {
            StartCoroutine(DisappearEnumerator());
            IEnumerator DisappearEnumerator()
            {
                for (int i = 0; i < 15; i++)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    yield return new WaitForSeconds(1f / 20);
                }
                spriteRenderer.enabled = false;
                finish();
            }
        }

        private void PlayAttackEffects()
        {
            attackEffect1.Play();
            attackEffect2.Play();
        }
    }
}
