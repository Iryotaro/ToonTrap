using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class AttackBehaviour : MonoBehaviour
    {
        public AttackableObjectId id { get; private set; }
        protected AttackableObjectEvents events { get; private set; }
        protected AttackableObjectApplicationService attackableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        private bool attackToOnlyPlayer;
        private Player player;

        protected void SetId(AttackableObjectId id, bool attackToOnlyPlayer)
        {
            this.id = id;
            this.attackToOnlyPlayer = attackToOnlyPlayer;
            events = attackableObjectApplicationService.GetEvents(id);

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x => player = x.player)
                .AddTo(this);

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
            attackableObjectApplicationService.ReAttack(id);
        }
    }
}
