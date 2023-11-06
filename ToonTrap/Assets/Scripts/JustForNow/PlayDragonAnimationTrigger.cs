using Ryocatusn.Characters;
using System.Collections;
using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Games;
using Ryocatusn.TileTransforms;
using Ryocatusn.Audio;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class PlayDragonAnimationTrigger : MonoBehaviour
    {
        [SerializeField]
        private Dragon dragon;
        [SerializeField]
        private SE damageSE;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private JankenableObjectApplicationService jankenableObjectApplicationService;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .Subscribe(_ =>
                {
                    gameManager.gameContains.player.inputMaster.SetActiveAll(false);
                    TileTransform tileTransform = gameManager.gameContains.player.tileTransform;
                    tileTransform.SetMovement(new MoveTranslate(tileTransform.tilePosition.Get(), new TileDirection(TileDirection.Direction.Right)), new MoveRate(7), TileTransform.SetMovementMode.Force);

                    jankenableObjectApplicationService.ChangeShape(gameManager.gameContains.player.id, Janken.Hand.Shape.Scissors);

                    StartCoroutine(A());

                    IEnumerator A()
                    {
                        yield return new WaitForSeconds(2);
                        dragon.dragonAnimator.PlayAnimations(new DragonAnimator.AnimationType[] { DragonAnimator.AnimationType.Appear, DragonAnimator.AnimationType.FirstAppearance, DragonAnimator.AnimationType.Provocation });
                        yield return new WaitForSeconds(8);
                        dragon.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Attack1);
                        yield return new WaitForSeconds(3);
                        dragon.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Damage);
                        new SEPlayer(gameObject).Play(damageSE);
                        yield return new WaitForSeconds(2);
                        dragon.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Disappear);
                        yield return new WaitForSeconds(3);
                        gameManager.nowStageManager.Clear();
                    }
                });
        }
    }
}
