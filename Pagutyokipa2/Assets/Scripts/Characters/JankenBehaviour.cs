using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn;

public class JankenBehaviour : MonoBehaviour
{
    public JankenableObjectId id { get; private set; }
    protected JankenableObjectEvents events { get; private set; }
    protected JankenableObjectApplicationService jankenableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();
    protected AttackableObjectApplicationService attackableObjectApplicationService { get; } = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

    protected void Create(JankenableObjectCreateCommand command)
    {
        id = jankenableObjectApplicationService.Create(command);
        events = jankenableObjectApplicationService.GetEvents(id);
    }
    protected AttackableObjectId Create(AttackableObjectCreateCommand command)
    {
        return attackableObjectApplicationService.Create(command);
    }
    protected void Attack(AttackableObjectId id, IReceiveAttack[] receiveAttacks)
    {
        foreach (IReceiveAttack receiveAttack in receiveAttacks)
        {
            attackableObjectApplicationService.Attack(id, receiveAttack);
        }
    }
}
