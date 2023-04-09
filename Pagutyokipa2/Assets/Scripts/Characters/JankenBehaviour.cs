using Microsoft.Extensions.DependencyInjection;
using Ryocatusn;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class JankenBehaviour : MonoBehaviour
{
    public JankenableObjectId id { get; private set; }
    public JankenableObjectEvents events { get; private set; }
    protected JankenableObjectApplicationService jankenableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();
    protected AttackableObjectApplicationService attackableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

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
    protected AttackableObjectId CreateAttackableObject(AttackableObjectCreateCommand command)
    {
        return attackableObjectApplicationService.Create(command);
    }
    protected void AttackTrigger(AttackableObjectCreateCommand command, IReceiveAttack[] receiveAttacks = null)
    {
        jankenableObjectApplicationService.AttackTrigger(id, command, receiveAttacks);
    }
    protected void Attack(AttackableObjectId id, IReceiveAttack[] receiveAttacks)
    {
        foreach (IReceiveAttack receiveAttack in receiveAttacks)
        {
            attackableObjectApplicationService.Attack(id, receiveAttack);
        }
    }
}
