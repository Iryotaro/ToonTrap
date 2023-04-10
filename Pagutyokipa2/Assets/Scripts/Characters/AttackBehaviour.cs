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

        protected void SetId(AttackableObjectId id)
        {
            this.id = id;
            events = attackableObjectApplicationService.GetEvents(id);

            events.OwnerDieEvent
                .Subscribe(_ => HandlOwnerDie())
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
            attackableObjectApplicationService.Attack(id, receiveAttack);
        }
        protected void ReAttack()
        {
            attackableObjectApplicationService.ReAttack(id);
        }

        protected virtual void HandlOwnerDie()
        {
            Destroy(gameObject);
        }
    }
}
