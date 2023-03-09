using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using DG.Tweening;
using Ryocatusn.Games.Stages;
using Ryocatusn.TileTransforms;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(TileTransform))]
    public class SimpleEnemy : MonoBehaviour, IReceiveAttack, IHandForSpriteEditor
    {
        public JankenableObjectId id { get; private set; }
        protected JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

        protected SEPlayer sePlayer { get; private set; }

        [SerializeField, Min(1)]
        private int hp;
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private HandSprites handSprites;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private SE takeDamageSE;
        [SerializeField]
        private SE dieSE;

        private void Awake()
        {
            GetComponent<Renderer>().OnBecameVisibleAsObservable()
                .Subscribe(_ => 
                {
                    enabled = true;
                });

            enabled = false;
        }

        private void Start()
        {
            id = Create(new Hp(hp), shape, StageManager.activeStage.id);
            JankenableObjectEvents events = jankenableObjectApplicationService.GetEvents(id);
            SetEventHandlers(events);

            TileTransform tileTransform = GetComponent<TileTransform>();
            tileTransform.ChangeTilemap(StageManager.activeStage.roads, transform.position);

            sePlayer = new SEPlayer(gameObject);

            Started();

            Tween tween = transform.DOScale(Vector2.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0, 0.6f)).SetLink(gameObject);
            events.DieEvent.Subscribe(_ => tween.Kill()).AddTo(this);
        }
        private void OnDestroy()
        {
            jankenableObjectApplicationService.Delete(id);
            OnDestroyed();
        }

        protected virtual void Started()
        {

        }
        protected virtual void OnDestroyed()
        {

        }

        protected virtual JankenableObjectId Create(Hp hp, Hand.Shape shape, StageId stageId)
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(hp, shape, stageId);
            return jankenableObjectApplicationService.Create(command);
        }
        protected virtual void SetEventHandlers(JankenableObjectEvents events)
        {
            events.DieEvent.Subscribe(_ => Delete()).AddTo(this);
            events.TakeDamageEvent.Subscribe(_ => sePlayer.Play(takeDamageSE)).AddTo(this);
        }

        protected virtual void Delete()
        {
            sePlayer.Play(dieSE);
            jankenableObjectApplicationService.Delete(id);

            Sequence sequence = DOTween.Sequence();
            sequence
                .SetLink(gameObject)
                .Append(transform.DOScale(new Vector3(1.3f, 1.5f, 1), 0.2f))
                .Append(transform.DOScale(Vector3.zero, 0.2f))
                .OnComplete(() => Destroy(gameObject));
        }

        public SpriteRenderer[] GetSpriteRenderers()
        {
            return new SpriteRenderer[1] { spriteRenderer };
        }
        public Hand.Shape GetHandShape()
        {
            return shape;
        }
        public HandSprites GetHandSprites()
        {
            return handSprites;
        }
    }
}
