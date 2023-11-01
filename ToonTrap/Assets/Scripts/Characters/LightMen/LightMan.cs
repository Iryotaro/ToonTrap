using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Lights;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    public class LightMan : JankenBehaviour
    {
        private JankenableObjectId playerId;

        [SerializeField]
        private SpotLight spotLight;
        [SerializeField]
        private Transform spotLightExtraPosition;
        [SerializeField]
        private JankenSprites helmets;
        [SerializeField]
        private SpriteRenderer helmetSpriteRenderer;
        [SerializeField]
        private JankenSprites hands;
        [SerializeField]
        private SpriteRenderer handSpriteRenderer;
        [SerializeField]
        [Min(1)]
        private float changeShapeWaitTime;

        private Vector2 randomPosition;
        private bool appeared = false;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            TurnOffLight();

            MoveLight();

            Create(new Hp(1), Hand.GetRandomShape());

            playerId = gameManager.gameContains.player.id;
            JankenableObjectEvents playerEvents = gameManager.gameContains.player.events;

            playerEvents.ChangeShapeEvent
                .Subscribe(x => SetLight())
                .AddTo(this);

            events.ChangeShapeEvent
                .Subscribe(x => SetLight())
                .AddTo(this);

            events.ChangeShapeEvent
                .Subscribe(x => ChangeShapeViews(x))
                .AddTo(this);

            StartCoroutine(ChangeShapeEnumerator());
            IEnumerator ChangeShapeEnumerator()
            {
                while (true)
                {
                    yield return new WaitForSeconds(changeShapeWaitTime);
                    ChangeShape(Hand.GetRandomShape());
                }
            }
        }

        private void Update()
        {
            spotLight.TurnOnExtra(spotLightExtraPosition.position);
        }

        public void Appear()
        {
            animator.SetTrigger("Appear");
        }
        public void Disappear()
        {
            animator.SetTrigger("Disappear");
        }

        public void AppearedCallbackFromAnimation()
        {
            appeared = true;
            SetLight();
        }
        public void DisappearedCallbackFromAnimation()
        {
            appeared = false;
            TurnOffLight();
        }

        private bool IsAllowedToSetLight()
        {
            return appeared;
        }
        private void SetLight()
        {
            if (!IsAllowedToSetLight()) return;

            Hand.Shape playerShape = jankenableObjectApplicationService.Get(playerId).shape;
            Hand.Shape shape = GetData().shape;

            if (playerShape == shape) TurnOnLight();
            else TurnOffLight();
        }

        private void TurnOnLight()
        {
            spotLight.on = true;
        }
        private void TurnOffLight()
        {
            spotLight.on = false;
        }

        private void ChangeShape(Hand.Shape shape)
        {
            jankenableObjectApplicationService.ChangeShape(id, shape);
        }
        private void ChangeShapeViews(Hand.Shape shape)
        {
            Sprite helmet = helmets.GetAsset(shape);
            Sprite hand = hands.GetAsset(shape);

            helmetSpriteRenderer.sprite = helmet;
            handSpriteRenderer.sprite = hand;
        }

        private void MoveLight()
        {
            StartCoroutine(ChangeRandomPosition());
            IEnumerator ChangeRandomPosition()
            {
                bool setted = false;
                gameManager.SetStageEvent
                    .Subscribe(_ => setted = true)
                    .AddTo(this);

                yield return new WaitUntil(() => setted);

                GameCamera gameCamera = gameManager.gameContains.gameCamera;

                while (true)
                {
                    Vector2 randomWorldPositionOnGame = gameCamera.camera.ViewportToWorldPoint(new Vector2(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f)));
                    randomPosition = gameManager.GetWorldPositoinOnFinalResult(randomWorldPositionOnGame);
                    yield return new WaitForSeconds(2);
                }
            }

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {

                    Vector2 currentVelocity = Vector2.zero;
                    spotLight.transform.position = Vector2.SmoothDamp(spotLight.transform.position, randomPosition, ref currentVelocity, 0.2f);
                });
        }
    }
}
