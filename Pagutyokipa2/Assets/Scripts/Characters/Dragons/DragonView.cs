using Anima2D;
using DG.Tweening;
using Ryocatusn.Janken;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SpriteMeshInstance))]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class DragonView : MonoBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Dragon dragon;
        [SerializeField]
        private JankenSpriteMeshes jankenSpriteMeshes;
        [SerializeField]
        private IkCCD2D ik;

        [NonSerialized]
        public SkinnedMeshRenderer skinnedMeshRenderer;

        private Player player;
        private Vector2 gap;

        public void SetUp()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            Move();

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x => player = x.player);
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

        public Hand.Shape GetShape()
        {
            if (dragon == null) return Hand.Shape.Rock;
            return dragon.shape;
        }
        public void UpdateView(Hand.Shape shape)
        {
            if (jankenSpriteMeshes.TryGetRenderer(out SpriteMeshInstance spriteMeshInstance, this))
            {
                spriteMeshInstance.spriteMesh = jankenSpriteMeshes.GetAsset(shape);
            }
        }
    }
}
