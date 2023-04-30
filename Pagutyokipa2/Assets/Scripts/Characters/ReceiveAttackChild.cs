using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using Microsoft.Extensions.DependencyInjection;

namespace Ryocatusn.Characters
{
    public class ReceiveAttackChild : MonoBehaviour, IReceiveAttack
    {
        private AttackableObjectApplicationService attackableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        [SerializeField]
        private JankenBehaviour jankenBehaviour;
        [SerializeField]
        private AttackBehaviour attackBehaviour;

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
