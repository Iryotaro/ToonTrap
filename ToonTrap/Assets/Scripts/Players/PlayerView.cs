using DG.Tweening;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.TileTransforms;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransform))]
    public class PlayerView : MonoBehaviour
    {
        [Inject]
        protected JankenableObjectApplicationService jankenableObjectApplicationService;
        protected JankenableObjectId id;
        protected JankenableObjectEvents events;
        protected Player player;

        [Inject]
        private GameManager gameManager;

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private JankenSprites jankenSprites;
        [SerializeField]
        private GameObject boomEffect;

        private TileTransform tileTransform;

        private float lastChangeSpriteTime;
        private Action invincibleFinishEvent;

        public void Setup(Player player)
        {
            tileTransform = GetComponent<TileTransform>();

            this.player = player;
            id = this.player.id;

            events = jankenableObjectApplicationService.GetEvents(id);

            events.ChangeShapeEvent
                .Subscribe(x => ChangeShape(x))
                .AddTo(this);

            events.TakeDamageEvent
                .Subscribe(_ => invincibleFinishEvent = Invincible());

            events.TakeDamageEvent
                .Subscribe(_ => TakeDamage());

            events.FinishInvincibleTime
                .Subscribe(_ => invincibleFinishEvent?.Invoke());

            ChangeShape(jankenableObjectApplicationService.Get(id).shape);

            player.gameObject.UpdateAsObservable()
                .Subscribe(_ => ChangeAngle(player));
        }

        private void ChangeShape(Hand.Shape shape)
        {
            spriteRenderer.sprite = jankenSprites.GetAsset(shape);
        }

        private void ChangeAngle(Player player)
        {
            if (!player.inputMaster.isAllowedMove) return;

            player.transform.rotation = tileTransform.tileDirection.GetRotation();
        }

        private Action Invincible()
        {
            IDisposable disaposable = this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Time.fixedTime - lastChangeSpriteTime > 0.2f)
                    {
                        lastChangeSpriteTime = Time.fixedTime;
                        spriteRenderer.enabled = !spriteRenderer.enabled;
                    }
                });

            Action finish = null;
            finish += () =>
            {
                disaposable.Dispose();
                spriteRenderer.enabled = true;
            };

            return finish;
        }

        private void TakeDamage()
        {
            CreateBoomEffect();
            gameManager.gameContains.gameCamera.Impulse();
        }

        private void CreateBoomEffect()
        {
            Instantiate(boomEffect, transform.position, Quaternion.identity);
        }
    }
}
