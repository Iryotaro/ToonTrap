using UnityEngine;
using Ryocatusn.Janken.AttackableObjects;
using DG.Tweening;
using UniRx;
using System;
using Ryocatusn.Audio;
using Zenject;
using Ryocatusn.Games;

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

        [Inject]
        private GameManager gameManager;

        private Subject<Unit> shotSubject = new Subject<Unit>();

        public IObservable<Unit> ShotSubject => shotSubject;

        public void SetUp(AttackableObjectId attackableObjectId, int chaseTime)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            DOTween.Sequence()
                .Append(Appear())
                .AppendInterval(chaseTime)
                .Append(Disappear())
                .OnComplete(() => shotSubject.OnNext(Unit.Default));
        }
        private void OnDestroy()
        {
            shotSubject.Dispose();
        }

        private Tween Appear()
        {
            return DOTween.Sequence()
                .AppendCallback(() => sePlayer.Play(appearSE))
                .AppendCallback(() => spriteRenderer.enabled = true)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = false)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = true)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = false)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = true);
        }

        private Tween Disappear()
        {
            return DOTween.Sequence()
                .AppendCallback(() => spriteRenderer.enabled = false)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = true)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = false)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = true)
                .AppendInterval(1f / 4)
                .AppendCallback(() => spriteRenderer.enabled = false);
        }
    }
}
