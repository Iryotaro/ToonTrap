using Ryocatusn.Games.Stages;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class JankenableObjectApplicationService
    {
        private IJankenableObjectRepository jankenableObjectRepository { get; }
        private HandApplicationService handApplicationService { get; }
        private AttackableObjectApplicationService attackableObjectApplicationService { get; }
        private StageApplicationService stageApplicationService { get; }

        public JankenableObjectApplicationService(IJankenableObjectRepository jankenableObjectRepository, HandApplicationService handApplicationService, AttackableObjectApplicationService attackableObjectApplicationService, StageApplicationService stageApplicationService)
        {
            this.jankenableObjectRepository = jankenableObjectRepository;
            this.handApplicationService = handApplicationService;
            this.attackableObjectApplicationService = attackableObjectApplicationService;
            this.stageApplicationService = stageApplicationService;
        }
        public JankenableObjectId Create(JankenableObjectCreateCommand command)
        {
            HandId handId = handApplicationService.Create(command.shape);

            JankenableObject jankenableObject = new JankenableObject(handId, command.hp, command.invincibleTime);
            jankenableObjectRepository.Save(jankenableObject);

            if (command.stageId != null)
            {
                StageEvents events = stageApplicationService.GetEvents(command.stageId);
                events.ClearEvent.ObserveOn(Scheduler.MainThreadEndOfFrame).Subscribe(_ => Delete(jankenableObject.id));
                events.OverEvent.ObserveOn(Scheduler.MainThreadEndOfFrame).Subscribe(_ => Delete(jankenableObject.id));
            }

            return jankenableObject.id;
        }
        public JankenableObjectData Get(JankenableObjectId id)
        {
            JankenableObject jankenableObject = jankenableObjectRepository.Find(id).Get();
            if (jankenableObject == null) throw new JankenException("JankenableObjectが見つかりません");

            return new JankenableObjectData
                (
                handApplicationService.Get(jankenableObject.handId).shape,
                jankenableObject.handId,
                jankenableObject.hp,
                jankenableObject.winCombo
                );
        }
        public JankenableObjectEvents GetEvents(JankenableObjectId id)
        {
            JankenableObject jankenableObject = jankenableObjectRepository.Find(id).Get();
            if (jankenableObject == null) throw new JankenException("JankenableObjectが見つかりません");

            return new JankenableObjectEvents
                (
                handApplicationService.GetEvents(jankenableObject.handId).ChangeShapeEvent,
                handApplicationService.GetEvents(jankenableObject.handId).JankenReverseEvent,
                jankenableObject.TakeDamageEvent,
                jankenableObject.FinishInvincibleTime,
                jankenableObject.ResetHpEvent,
                jankenableObject.ChangeHpEvent,
                jankenableObject.DieEvent,
                jankenableObject.AttackTriggerEvent,
                jankenableObject.DoJankenEvent,
                jankenableObject.WinEvent,
                jankenableObject.LoseEvent,
                jankenableObject.DrawEvent,
                jankenableObject.AttackerWinEvent,
                jankenableObject.AttackerLoseEvent,
                jankenableObject.AttackerDrawEvent,
                jankenableObject.VictimWinEvent,
                jankenableObject.VictimLoseEvent,
                jankenableObject.VictimDrawEvent
                );
        }
        public void ChangeShape(JankenableObjectId id, Hand.Shape shape)
        {
            jankenableObjectRepository.Find(id).Match
                (
                Some: x => handApplicationService.ChangeShape(x.handId, shape),
                None: () => throw new JankenException("JankenableObjectが見つかりません")
                );
        }
        public void AttackTrigger(JankenableObjectId id, AttackableObjectCreateCommand command, IReceiveAttack[] receiveAttacks = null)
        {
            AttackableObjectId attackableObjectId = attackableObjectApplicationService.Create(command);

            jankenableObjectRepository.Find(id).Match
                (
                Some: x => x.AttackTrigger(attackableObjectId, receiveAttacks),
                None: () => throw new JankenException("JankenableObjectが見つかりません")
                );
        }
        public void ResetHp(JankenableObjectId id)
        {
            jankenableObjectRepository.Find(id).Match
                (
                Some: x => x.ResetHp(),
                None: () => throw new JankenException("JankenableObjectが見つかりません")
                );
        }
        public void ChangeHp(JankenableObjectId id, Hp hp)
        {
            jankenableObjectRepository.Find(id).Match
                (
                Some: x => x.ChangeHp(hp),
                None: () => throw new JankenException("JankenableObjectが見つかりません")
                );
        }
        public bool IsEnable(JankenableObjectId id)
        {
            JankenableObject jankenableObject = jankenableObjectRepository.Find(id).Get();

            return jankenableObject != null;
        }
        public void Delete(JankenableObjectId id)
        {
            jankenableObjectRepository.Find(id).Match
                (
                Some: x =>
                {
                    x.Delete();
                    handApplicationService.Delete(x.handId);
                    jankenableObjectRepository.Delete(id);
                }
                );
        }
    }
}
