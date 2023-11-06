using Ryocatusn.Games;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class AttackBehaviour : MonoBehaviour
    {
        public AttackableObjectId id { get; private set; }
        protected AttackableObjectEvents events { get; private set; }

        [Inject]
        protected AttackableObjectApplicationService attackableObjectApplicationService { get; }
        [Inject]
        protected GameManager gameManager;

        private bool attackToOnlyPlayer;
        private Player player;

        private void OnDestroy()
        {
            if (attackableObjectApplicationService == null) return;
            attackableObjectApplicationService.Delete(id);
        }

        protected void SetId(AttackableObjectId id, bool attackToOnlyPlayer)
        {
            this.id = id;
            this.attackToOnlyPlayer = attackToOnlyPlayer;
            events = attackableObjectApplicationService.GetEvents(id);

            player = gameManager.gameContains.player;

            this.OnDestroyAsObservable()
                .Subscribe(_ => attackableObjectApplicationService.Delete(id));
        }
        protected AttackableObjectData Get()
        {
            return attackableObjectApplicationService.Get(id);
        }
        protected void Attack(IReceiveAttack receiveAttack)
        {
            if (attackToOnlyPlayer)
            {
                if (!player.GetId().Equals(receiveAttack.GetId())) return;
            }

            if (!attackableObjectApplicationService.IsEnable(id)) return;
            attackableObjectApplicationService.Attack(id, receiveAttack);
        }
        protected void ReAttack()
        {
            if (attackableObjectApplicationService.IsEnable(id)) attackableObjectApplicationService.ReAttack(id);
        }
    }
}
