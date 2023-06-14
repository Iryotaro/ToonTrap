using Ryocatusn.Janken.JankenableObjects;

public interface IReceiveAttack
{
    JankenableObjectId GetId();
    public bool isAllowedToReceiveAttack { get; }
}
