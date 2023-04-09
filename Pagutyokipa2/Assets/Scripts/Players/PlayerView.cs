using DG.Tweening;
using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.TileTransforms;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransform))]
    public class PlayerView : MonoBehaviour
    {
        private JankenableObjectId id;
        private Player player;

        [SerializeField]
        private GameCamera gameCamera;

        private TileTransform tileTransform;

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private JankenSprites handSprites;

        private TileDirection direction = new TileDirection(TileDirection.Direction.Up);

        private bool invincible = false;
        private float lastChangeSpriteTime;

        public void StartView(Player player)
        {
            JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

            this.player = player;
            id = this.player.id;

            tileTransform = GetComponent<TileTransform>();

            JankenableObjectEvents events = jankenableObjectApplicationService.GetEvents(id);

            events.ChangeShapeEvent
                .Subscribe(x => ChangeShape(x))
                .AddTo(this);

            events.AttackTriggerEvent
                .Subscribe(_ => Shot())
                .AddTo(this);

            events.TakeDamageEvent
                .Subscribe(_ => invincible = true);

            events.TakeDamageEvent
                .Subscribe(_ => TakeDamage());

            events.FinishInvincibleTime
                .Subscribe(_ => { invincible = false; spriteRenderer.enabled = true; });

            events.DieEvent
                .Subscribe(_ => Die());

            ChangeShape(jankenableObjectApplicationService.Get(id).shape);

            player.gameObject.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (invincible) Invincible();
                    if (!direction.Equals(tileTransform.tileDirection)) ChangeLookDirection(tileTransform.tileDirection);
                });
        }

        private void ChangeShape(Hand.Shape shape)
        {
            switch (shape)
            {
                case Hand.Shape.Rock:
                    spriteRenderer.sprite = handSprites.rockSprite;
                    break;
                case Hand.Shape.Scissors:
                    spriteRenderer.sprite = handSprites.scissorsSprite;
                    break;
                case Hand.Shape.Paper:
                    spriteRenderer.sprite = handSprites.paperSprite;
                    break;
            }
        }
        private void Shot()
        {

        }
        private void TakeDamage()
        {
            CreateLensDistortionSequence();
        }
        private void ChangeLookDirection(TileDirection tileDirection)
        {
            if (!player.inputMaster.isAllowedMove) return;

            direction = tileDirection;

            transform.eulerAngles = tileDirection.value switch
            {
                TileDirection.Direction.Up => Vector3.zero,
                TileDirection.Direction.Down => new Vector3(0, 0, 180),
                TileDirection.Direction.Left => new Vector3(0, 0, 90),
                TileDirection.Direction.Right => new Vector3(0, 0, -90),
                _ => Vector3.zero
            };
        }
        private void Invincible()
        {
            if (Time.fixedTime - lastChangeSpriteTime > 0.2f)
            {
                lastChangeSpriteTime = Time.fixedTime;
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
        private void Die()
        {
            CreateLensDistortionSequence();
        }

        private Sequence CreateLensDistortionSequence()
        {
            Sequence sequence = DOTween.Sequence();

            return sequence
                .SetLink(gameCamera.gameObject)
                .Append(
                DOTween.To
                (
                    () => gameCamera.lensDistortion.intensity.value,
                    x => gameCamera.lensDistortion.intensity.value = x,
                    -0.3f,
                    0.6f
                    )
                )
                .AppendInterval(0.3f)
                .Append(
                DOTween.To
                (
                    () => gameCamera.lensDistortion.intensity.value,
                    x => gameCamera.lensDistortion.intensity.value = x,
                    0,
                    0.6f
                    )
                );
        }
    }
}
