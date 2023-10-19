using Anima2D;
using DG.Tweening;
using Ryocatusn.Games;
using Ryocatusn.Photographers;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class DragonMiniView : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        [SerializeField]
        private IkCCD2D ik;
        [SerializeField]
        private DragonMiniMouth dragonMouth;

        [NonSerialized]
        public Transform shotPoint;

        private SkinnedMeshRenderer skinnedMeshRenderer;

        private Player player;
        private Vector2 gap;

        private Subject<Unit> attackTriggerEvent = new Subject<Unit>();

        public IObservable<Unit> AttackTriggerEvent => attackTriggerEvent;


        public void SetUp()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            shotPoint = dragonMouth.shotPoint;

            Move();

            dragonMouth.AttackTriggerEvent.Subscribe(_ => attackTriggerEvent.OnNext(Unit.Default));

            player = gameManager.gameContains.player;
        }
        private void OnDestroy()
        {
            attackTriggerEvent.Dispose();
        }

        private void Move()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (player == null) return;
                    Vector2 lookAt = player.transform.position;
                    ik.transform.position = lookAt + gap;
                });

            StartCoroutine(ChangeGap());

            IEnumerator ChangeGap()
            {
                while (true)
                {
                    bool finish = false;

                    Vector2 newGap = new Vector2(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));

                    DOTween.To
                        (
                        () => gap,
                        x => gap = x,
                        newGap,
                        0.5f
                        )
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => finish = true);

                    yield return new WaitUntil(() => finish);
                }
            }
        }
        public void StartAttackAnimation()
        {
            dragonMouth.StartAttackAnimation();
        }
        
        public bool IsVisible()
        {
            return skinnedMeshRenderer.isVisible;
        }
    }
}
