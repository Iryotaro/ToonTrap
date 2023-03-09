using System;
using System.Threading.Tasks;
using UniRx;
using Ryocatusn.Janken.AttackableObjects;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class JankenableObject : IEquatable<JankenableObject>
    {
        public JankenableObjectId id { get; }

        public HandId handId { get; }

        public Hp hp { get; private set; }
        public InvincibleTime invincibleTime { get; }
        public int winCombo { get; private set; } = 0;

        private bool invincible;

        private Subject<Hp> takeDamageEvent = new Subject<Hp>();
        private Subject<Unit> finishInvincibleTime = new Subject<Unit>();
        private Subject<Hp> resetHpEvent = new Subject<Hp>();
        private Subject<Hp> changeHpEvent = new Subject<Hp>();
        private Subject<Unit> dieEvent = new Subject<Unit>();
        private Subject<(AttackableObjectId id, IReceiveAttack[] receiveAttacks)> attackTriggerEvent = new Subject<(AttackableObjectId, IReceiveAttack[])>();
        private Subject<AttackableObjectId> returnAttackTriggerEvent = new Subject<AttackableObjectId>();

        private Subject<Unit> doJankenEvent = new Subject<Unit>();
        private Subject<Unit> winEvent = new Subject<Unit>();
        private Subject<Unit> loseEvent = new Subject<Unit>();
        private Subject<Unit> drawEvent = new Subject<Unit>();
        private Subject<Unit> attackerWinEvent = new Subject<Unit>();
        private Subject<Unit> attackerLoseEvent = new Subject<Unit>();
        private Subject<Unit> attackerDrawEvent = new Subject<Unit>();
        private Subject<Unit> victimWinEvent = new Subject<Unit>();
        private Subject<Unit> victimLoseEvent = new Subject<Unit>();
        private Subject<Unit> victimDrawEvent = new Subject<Unit>();

        public IObservable<Hp> TakeDamageEvent => takeDamageEvent;
        public IObservable<Unit> FinishInvincibleTime => finishInvincibleTime;
        public IObservable<Hp> ResetHpEvent => resetHpEvent;
        public IObservable<Hp> ChangeHpEvent => changeHpEvent;
        public IObservable<Unit> DieEvent => dieEvent;
        public IObservable<(AttackableObjectId id, IReceiveAttack[] targets)> AttackTriggerEvent => attackTriggerEvent;
        public IObservable<AttackableObjectId> ReturnAttackTriggerEvent => returnAttackTriggerEvent;

        public IObservable<Unit> DoJankenEvent => doJankenEvent;
        public IObservable<Unit> WinEvent => winEvent;
        public IObservable<Unit> LoseEvent => loseEvent;
        public IObservable<Unit> DrawEvent => drawEvent;
        public IObservable<Unit> AttackerWinEvent => attackerWinEvent;
        public IObservable<Unit> AttackerLoseEvent => attackerLoseEvent;
        public IObservable<Unit> AttackerDrawEvent => attackerDrawEvent;
        public IObservable<Unit> VictimWinEvent => victimWinEvent;
        public IObservable<Unit> VictimLoseEvent => victimLoseEvent;
        public IObservable<Unit> VictimDrawEvent => victimDrawEvent;

        public JankenableObject(HandId handId, Hp hp, InvincibleTime invincibleTime = null)
        {
            id = new JankenableObjectId(Guid.NewGuid().ToString());

            this.handId = handId;

            this.hp = hp;
            this.invincibleTime = invincibleTime != null ? invincibleTime : new InvincibleTime(0);
        }

        public void AttackTrigger(AttackableObjectId attackableObjectId, IReceiveAttack[] receiveAttacks = null)
        {
            attackTriggerEvent.OnNext((attackableObjectId, receiveAttacks));
        }
        public void ReAttackTrigger(AttackableObjectId attackableObjectId)
        {
            returnAttackTriggerEvent.OnNext(attackableObjectId);
        }

        public void TakeDamage(Atk atk)
        {
            if (hp.value == 0) return;
            if (invincible) return;
            invincible = true;
            FinishInvincible();

            DecreaseHp(atk);
        }
        public void ResetHp()
        {
            hp = hp.Reset();
            resetHpEvent.OnNext(hp);
            changeHpEvent.OnNext(hp);
        }
        public void ChangeHp(Hp hp)
        {
            this.hp = hp;
            changeHpEvent.OnNext(hp);
        }

        private void DecreaseHp(Atk atk)
        {
            hp = hp.Decrease(atk);
            changeHpEvent.OnNext(hp);

            if (hp.value == 0) dieEvent.OnNext(Unit.Default);
            else takeDamageEvent.OnNext(hp);
        }
        private async void FinishInvincible()
        {
            await Task.Delay((int)(invincibleTime.value * 1000));
            finishInvincibleTime.OnNext(Unit.Default);
            invincible = false;
        }

        public void DoJanken()
        {
            doJankenEvent.OnNext(Unit.Default);
        }
        public void AttackerWin()
        {
            winCombo++;

            winEvent.OnNext(Unit.Default);
            attackerWinEvent.OnNext(Unit.Default);
        }
        public void AttackerLose()
        {
            winCombo = 0;
      
            loseEvent.OnNext(Unit.Default);
            attackerLoseEvent.OnNext(Unit.Default);
        }
        public void AttackerDraw()
        {
            drawEvent.OnNext(Unit.Default);
            attackerDrawEvent.OnNext(Unit.Default);
        }

        public void VictimWin()
        {
            winCombo++;

            winEvent.OnNext(Unit.Default);
            victimWinEvent.OnNext(Unit.Default);
        }
        public void VictimLose()
        {
            winCombo = 0;

            loseEvent.OnNext(Unit.Default);
            victimLoseEvent.OnNext(Unit.Default);
        }
        public void VictimDraw()
        {
            drawEvent.OnNext(Unit.Default);
            victimDrawEvent.OnNext(Unit.Default);
        }

        public void Delete()
        {
            takeDamageEvent.Dispose();
            resetHpEvent.Dispose();
            dieEvent.Dispose();
            attackTriggerEvent.Dispose();

            winEvent.Dispose();
            loseEvent.Dispose();
            drawEvent.Dispose();
            attackerWinEvent.Dispose();
            attackerLoseEvent.Dispose();
            attackerDrawEvent.Dispose();
            victimWinEvent.Dispose();
            victimLoseEvent.Dispose();
            victimDrawEvent.Dispose();
        }

        public bool Equals(JankenableObject other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(id, other.id);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((JankenableObject)obj);
        }
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
    }
}
