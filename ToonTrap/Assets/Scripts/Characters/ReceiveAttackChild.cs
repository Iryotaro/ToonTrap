using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class ReceiveAttackChild : MonoBehaviour, IReceiveAttack
    {
        [Inject]
        private AttackableObjectApplicationService attackableObjectApplicationService { get; }

        public JankenBehaviour jankenBehaviour;
        public AttackBehaviour attackBehaviour;

        public bool isAllowedToReceiveAttack { get; set; } = true;

        public JankenableObjectId GetId()
        {
            if (jankenBehaviour != null) return jankenBehaviour.id;
            else if (attackBehaviour != null)
            {
                if (!attackableObjectApplicationService.IsEnable(attackBehaviour.id)) return null;
                return attackableObjectApplicationService.Get(attackBehaviour.id).ownerId;
            }
            return null;
        }
    }
}
