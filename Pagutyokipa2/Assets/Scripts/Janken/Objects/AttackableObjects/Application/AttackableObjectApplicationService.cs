using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Util;
using UniRx;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectApplicationService
    {
        private IAttackableObjectRepository attackableObjectRepository { get; }
        private IJankenableObjectRepository jankenableObjectRepository { get; }
        private HandApplicationService handApplicationService { get; }

        public AttackableObjectApplicationService(IAttackableObjectRepository attackableObjectRepository, IJankenableObjectRepository jankenableObjectRepository, HandApplicationService handApplicationService)
        {
            this.attackableObjectRepository = attackableObjectRepository;
            this.jankenableObjectRepository = jankenableObjectRepository;
            this.handApplicationService = handApplicationService;
        }

        public AttackableObjectId Create(AttackableObjectCreateCommand command)
        {
            JankenableObject jankenableObject = null;
            jankenableObjectRepository.Find(command.ownerId).Match
                (
                Some: x => jankenableObject = x,
                None: () => throw new JankenException("JankenableObjectが見つかりません")
                );

            HandId handId = handApplicationService.Create(command.shape);

            AttackableObject attackableObject = new AttackableObject(jankenableObject, handId, command.atk);
            attackableObjectRepository.Save(attackableObject);

            attackableObject.OwnerDieEvent
                .Subscribe(_ => Delete(attackableObject.id));

            return attackableObject.id;
        }
        public AttackableObjectData Get(AttackableObjectId id)
        {
            Option<AttackableObject> attackableObject = attackableObjectRepository.Find(id);
            if (attackableObject.Get() == null) throw new JankenException("AttackableObjectが見つかりません");

            return new AttackableObjectData
                (
                attackableObject.Get().owner.id,
                handApplicationService.Get(attackableObject.Get().handId).shape,
                attackableObject.Get().handId,
                attackableObject.Get().atk,
                attackableObject.Get().allowedReAttack
                );
        }
        public AttackableObjectEvents GetEvents(AttackableObjectId id)
        {
            AttackableObject attackableObject = attackableObjectRepository.Find(id).Get();
            if (attackableObject == null) throw new JankenException("AttackableObjectが見つかりません");

            return new AttackableObjectEvents
                (
                attackableObject.WinEvent,
                attackableObject.LoseEvent,
                attackableObject.DrawEvent,
                attackableObject.ReAttackTriggerEvent,
                attackableObject.ReAttackEvent,
                attackableObject.OwnerDieEvent
                );
        }
        public void Attack(AttackableObjectId id, IReceiveAttack receiveAttack)
        {
            AttackableObject attackableObject = null;
            JankenableObject attacker = null;
            JankenableObject victim = null;

            attackableObjectRepository.Find(id).Match(Some: x => attackableObject = x, None: () => throw new JankenException("AttackableObjectが見つかりません"));
            attacker = attackableObject.owner;
            jankenableObjectRepository.Find(receiveAttack.GetId()).Match(Some: x => victim = x);
            if (victim == null) return;

            JankenResult result = handApplicationService.DoJanken(new HandId[2] { attackableObject.handId, victim.handId });

            if (result.IsDraw()) attackableObject.Draw(victim);
            if (result.IsWin(attackableObject.handId)) attackableObject.Win(victim);
            if (result.IsLose(attackableObject.handId)) attackableObject.Lose(victim);
        }
        public void ReAttack(AttackableObjectId id)
        {
            attackableObjectRepository.Find(id).Match
                (
                Some: x => x.ReAttack(),
                None: () => throw new JankenException("AttackableObjectが見つかりません")
                );
        }
        public bool IsEnable(AttackableObjectId id)
        {
            AttackableObject attackableObject = attackableObjectRepository.Find(id).Get();

            return attackableObject != null;
        }
        public void Delete(AttackableObjectId id)
        {
            attackableObjectRepository.Find(id).Match
                (
                Some: x =>
                {
                    x.Delete();
                    handApplicationService.Delete(x.handId);
                    attackableObjectRepository.Delete(id);
                }
                );
        }
    }
}
