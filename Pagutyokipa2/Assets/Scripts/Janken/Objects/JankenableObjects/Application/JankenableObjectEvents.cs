﻿using Ryocatusn.Janken.AttackableObjects;
using System;
using UniRx;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class JankenableObjectEvents
    {
        public IObservable<Hand.Shape> ChangeShapeEvent { get; }
        public IObservable<bool> JankenReverseEvent { get; }

        public IObservable<Hp> TakeDamageEvent { get; }
        public IObservable<Unit> FinishInvincibleTime { get; }
        public IObservable<Hp> ResetHpEvent { get; }
        public IObservable<Hp> ChangeHpEvent { get; }
        public IObservable<Unit> DieEvent { get; }
        public IObservable<(AttackableObjectId id, IReceiveAttack[] receiveAttacks)> AttackTriggerEvent { get; }

        public IObservable<Unit> DoJankenEvent { get; }
        public IObservable<Unit> WinEvent { get; }
        public IObservable<Unit> LoseEvent { get; }
        public IObservable<Unit> DrawEvent { get; }
        public IObservable<Unit> AttackerWinEvent { get; }
        public IObservable<Unit> AttackerLoseEvent { get; }
        public IObservable<Unit> AttackerDrawEvent { get; }
        public IObservable<Unit> VictimWinEvent { get; }
        public IObservable<Unit> VictimLoseEvent { get; }
        public IObservable<Unit> VictimDrawEvent { get; }

        public JankenableObjectEvents
            (
            IObservable<Hand.Shape> ChangeShapeEvent,
            IObservable<bool> JankenReverseEvent,
            IObservable<Hp> TakeDamageEvent,
            IObservable<Unit> FinishInvincibleTime,
            IObservable<Hp> ResetHpEvent,
            IObservable<Hp> ChangeHpEvent,
            IObservable<Unit> DieEvent,
            IObservable<(AttackableObjectId, IReceiveAttack[])> AttackTriggerEvent,
            IObservable<Unit> DoJankenEvent,
            IObservable<Unit> WinEvent,
            IObservable<Unit> LoseEvent,
            IObservable<Unit> DrawEvent,
            IObservable<Unit> AttackerWinEvent,
            IObservable<Unit> AttackerLoseEvent,
            IObservable<Unit> AttackerDrawEvent,
            IObservable<Unit> VictimWinEvent,
            IObservable<Unit> VictimLoseEvent,
            IObservable<Unit> VictimDrawEvent
            )
        {
            this.ChangeShapeEvent = ChangeShapeEvent;
            this.JankenReverseEvent = JankenReverseEvent;
            this.TakeDamageEvent = TakeDamageEvent;
            this.FinishInvincibleTime = FinishInvincibleTime;
            this.ResetHpEvent = ResetHpEvent;
            this.ChangeHpEvent = ChangeHpEvent;
            this.DieEvent = DieEvent;
            this.AttackTriggerEvent = AttackTriggerEvent;
            this.DoJankenEvent = DoJankenEvent;
            this.WinEvent = WinEvent;
            this.LoseEvent = LoseEvent;
            this.DrawEvent = DrawEvent;
            this.AttackerWinEvent = AttackerWinEvent;
            this.AttackerLoseEvent = AttackerLoseEvent;
            this.AttackerDrawEvent = AttackerDrawEvent;
            this.VictimWinEvent = VictimWinEvent;
            this.VictimLoseEvent = VictimLoseEvent;
            this.VictimDrawEvent = VictimDrawEvent;
        }
    }
}
