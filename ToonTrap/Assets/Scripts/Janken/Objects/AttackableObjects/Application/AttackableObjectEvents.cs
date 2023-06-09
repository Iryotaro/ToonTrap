using System;
using UniRx;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectEvents
    {
        public IObservable<Unit> WinEvent { get; }
        public IObservable<Unit> LoseEvent { get; }
        public IObservable<Unit> DrawEvent { get; }

        public IObservable<Unit> ReAttackTriggerEvent { get; }
        public IObservable<Unit> ReAttackEvent { get; }

        public IObservable<Unit> OwnerDieEvent { get; }

        public AttackableObjectEvents
            (
            IObservable<Unit> WinEvent,
            IObservable<Unit> LoseEvent,
            IObservable<Unit> DrawEvent,
            IObservable<Unit> ReAttackTriggerEvent,
            IObservable<Unit> ReAttackEvent,
            IObservable<Unit> OwnerDieEvent
            )
        {
            this.WinEvent = WinEvent;
            this.LoseEvent = LoseEvent;
            this.DrawEvent = DrawEvent;

            this.ReAttackTriggerEvent = ReAttackTriggerEvent;
            this.ReAttackEvent = ReAttackEvent;

            this.OwnerDieEvent = OwnerDieEvent;
        }
    }
}
