using UnityEngine;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using System;
using Ryocatusn.Audio;
using Zenject;
using Ryocatusn.Games;
using System.Collections;
using DG.Tweening;

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
        private SE warningSE;
        [SerializeField]
        private SE hitSE;

        [SerializeField]
        private GameObject bullet;
        [SerializeField]
        private Transform shotPoint;

        [SerializeField]
        private float bulletHitUpToTime = 0.6f;

        [SerializeField]
        private ParticleSystem hitEffect1;
        [SerializeField]
        private ParticleSystem hitEffect2;

        [Inject]
        private GameManager gameManager;

        private Subject<Unit> shotEvent = new Subject<Unit>();
        private Subject<Unit> hitEvent = new Subject<Unit>();

        public IObservable<Unit> ShotEvent => shotEvent;
        public IObservable<Unit> HitEvent => hitEvent;

        public void SetUp(AttackableObjectId attackableObjectId, int chaseTime)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            StartCoroutine(PlayEnumerator());
            IEnumerator PlayEnumerator()
            {
                sePlayer.Play(appearSE);

                float time = Time.fixedTime;
                warningSE = warningSE.ChangePitch(0.6f);
                while (Time.fixedTime - time < chaseTime * 0.6f)
                {
                    Flash();
                    yield return new WaitForSeconds(0.4f);
                }

                warningSE = warningSE.ChangePitch(1f);
                bool one = true;
                while (Time.fixedTime - time < chaseTime)
                {
                    Flash();
                    yield return new WaitForSeconds(0.1f);

                    if (chaseTime - (Time.fixedTime - time) < bulletHitUpToTime && one)
                    {
                        one = false;
                        Shot();
                    }
                }

                Hit();
            }
        }
        private void OnDestroy()
        {
            shotEvent.Dispose();
            hitEvent.Dispose();
        }

        private void Flash()
        {
            StartCoroutine(FlashEnumearator());
            IEnumerator FlashEnumearator()
            {
                sePlayer.Play(warningSE);
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.05f);
                spriteRenderer.enabled = true;
            }
        }
        private void Shot()
        {
            shotEvent.OnNext(Unit.Default);

            GameObject bullet = Instantiate(this.bullet, shotPoint.transform.position, Quaternion.identity);
            bullet.transform
                .DOMove(gameManager.gameContains.player.transform.position, bulletHitUpToTime)
                .SetEase(Ease.InQuart)
                .OnComplete(() => Destroy(bullet));
        }
        private void Hit()
        {
            sePlayer.Play(hitSE);
            spriteRenderer.enabled = false;
            hitEvent.OnNext(Unit.Default);
            PlayHitEffects();
        }

        private void PlayHitEffects()
        {
            hitEffect1.Play();
            hitEffect2.Play();
        }
    }
}
