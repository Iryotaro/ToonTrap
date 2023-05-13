using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class JankenBehaviour : NetworkBehaviour
    {
        public JankenableObjectId id { get; private set; }
        [Inject]
        protected JankenableObjectApplicationService jankenableObjectApplicationService { get; }
        public JankenableObjectEvents events { get; private set; }

        protected void Create(JankenableObjectCreateCommand command)
        {
            id = jankenableObjectApplicationService.Create(command);
            events = jankenableObjectApplicationService.GetEvents(id);

            this.OnDestroyAsObservable()
                .Subscribe(_ => jankenableObjectApplicationService.Delete(id));
        }
        protected JankenableObjectData GetData()
        {
            return jankenableObjectApplicationService.Get(id);
        }
        protected void AttackTrigger(AttackableObjectCreateCommand command, IReceiveAttack[] receiveAttacks = null)
        {
            jankenableObjectApplicationService.AttackTrigger(id, command, receiveAttacks);
        }
    }
}
