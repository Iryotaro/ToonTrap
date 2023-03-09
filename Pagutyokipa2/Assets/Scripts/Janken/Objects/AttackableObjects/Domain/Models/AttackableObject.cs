using System;
using UniRx;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObject : IEquatable<AttackableObject>
    {
        public AttackableObjectId id { get; }

        public JankenableObject owner { get; }
        public HandId handId { get; }
        
        public Atk atk { get; private set; }

        public bool allowedReAttack { get; private set; } = false;

        private Subject<Unit> winEvent = new Subject<Unit>();
        private Subject<Unit> loseEvent = new Subject<Unit>();
        private Subject<Unit> drawEvent = new Subject<Unit>();

        private Subject<Unit> reAttackTriggerEvent = new Subject<Unit>();
        private Subject<Unit> reAttackEvent = new Subject<Unit>();

        private Subject<Unit> ownerDieEvent = new Subject<Unit>();

        public IObservable<Unit> WinEvent => winEvent;
        public IObservable<Unit> LoseEvent => loseEvent;
        public IObservable<Unit> DrawEvent => drawEvent;

        public IObservable<Unit> ReAttackTriggerEvent => reAttackTriggerEvent;
        public IObservable<Unit> ReAttackEvent => reAttackEvent;

        public IObservable<Unit> OwnerDieEvent => ownerDieEvent;

        public AttackableObject(JankenableObject owner, HandId handId, Atk atk)
        {
            id = new AttackableObjectId(Guid.NewGuid().ToString());

            this.owner = owner;
            this.handId = handId;

            this.atk = atk;

            owner.DieEvent.Subscribe(_ => ownerDieEvent.OnNext(Unit.Default));
        }

        public void ChangeAtk(Atk atk)
        {
            this.atk = atk;
        }

        public void Win(JankenableObject victim)
        {
            owner.AttackerWin();
            victim.VictimLose();

            owner.DoJanken();
            victim.DoJanken();

            winEvent.OnNext(Unit.Default);
            victim.TakeDamage(atk);
        }
        public void Lose(JankenableObject victim)
        {
            owner.AttackerLose();
            victim.VictimWin();

            owner.DoJanken();
            victim.DoJanken();

            allowedReAttack = true;
            loseEvent.OnNext(Unit.Default);
            reAttackTriggerEvent.OnNext(Unit.Default);
        }
        public void Draw(JankenableObject victim)
        {
            owner.AttackerDraw();
            victim.VictimDraw();

            owner.DoJanken();
            victim.DoJanken();

            drawEvent.OnNext(Unit.Default);
        }

        public void ReAttack()
        {
            if (!allowedReAttack) throw new JankenException("ReAttackは許可されていません");

            reAttackEvent.OnNext(Unit.Default);
            
            owner.TakeDamage(atk);
        }

        public void Delete()
        {
            winEvent.Dispose();
            loseEvent.Dispose();
            drawEvent.Dispose();
            reAttackTriggerEvent.Dispose();
            reAttackEvent.Dispose();
        }

        public bool Equals(AttackableObject other)
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
            return Equals((AttackableObject)obj);
        }
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
    }
}
