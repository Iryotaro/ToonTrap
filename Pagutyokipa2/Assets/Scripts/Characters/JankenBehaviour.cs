using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class JankenBehaviour : MonoBehaviour
    {
        public JankenableObjectId id { get; private set; }
        public JankenableObjectEvents events { get; private set; }
        protected JankenableObjectApplicationService jankenableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

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
