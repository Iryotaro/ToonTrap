using UniRx;
using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Audio;
using Ryocatusn.Janken.AttackableObjects;
using Anima2D;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SpriteMeshInstance))]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class Dragon : JankenBehaviour, IReceiveAttack, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenSpriteMeshes jankenSpriteMeshes;
        [SerializeField]
        private Rigidbody2D ikRigid;

        private SkinnedMeshRenderer skinnedMeshRenderer;

        private void Start()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, StageManager.activeStage.id);
            Create(command);

            SEPlayer sePlayer = new SEPlayer(gameObject, skinnedMeshRenderer);

            events.AttackTriggerEvent.Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks));
        }
        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack[] receiveAttacks)
        {

        }

        public Hand.Shape GetShape()
        {
            return shape;
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
