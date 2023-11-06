using Ryocatusn.TileTransforms;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ryocatusn
{
    public class PlayerInputMaster : MonoBehaviour
    {
        private InputMaster input;

        public bool isAllowedToMove { get; private set; } = true;
        public bool isAllowedToAttack { get; private set; } = true;
        public bool isAllowedToChangeShape { get; private set; } = true;

        [NonSerialized]
        public Key[] rockKeys = new Key[] { Key.R, Key.E, Key.W, Key.A };
        [NonSerialized]
        public Key[] scissorsKeys = new Key[] { Key.W, Key.A };

        private Subject<Unit> moveEvent = new Subject<Unit>();
        //private Subject<Unit> specialEvent = new Subject<Unit>();
        private Subject<Unit> cancelMoveEvent = new Subject<Unit>();
        private Subject<TileDirection> changeDirectionEvent = new Subject<TileDirection>();
        private Subject<float> attackEvent = new Subject<float>();
        private Subject<Unit> changeShapeEvent = new Subject<Unit>();

        public IObservable<Unit> MoveEvent { get; private set; }
        //public IObservable<Unit> SpecialEvent { get; private set; }
        public IObservable<Unit> CancelMoveEvent { get; private set; }
        public IObservable<TileDirection> ChangeDirectionEvent { get; private set; }
        public IObservable<Unit> AttackEvent { get; private set; }
        public IObservable<Unit> ChangeShapeEvent { get; private set; }

        private void OnEnable()
        {
            input.Player.Enable();
        }
        private void OnDisable()
        {
            input.Player.Disable();

            moveEvent.Dispose();
            cancelMoveEvent.Dispose();
            changeDirectionEvent.Dispose();
            attackEvent.Dispose();
            changeShapeEvent.Dispose();
        }

        private void Awake()
        {
            input = new InputMaster();

            input.Player.Move.performed += ctx => moveEvent.OnNext(Unit.Default);
            input.Player.Move.canceled += _ => cancelMoveEvent.OnNext(Unit.Default);
            input.Player.Move.performed += ctx => changeDirectionEvent.OnNext(new TileDirection(ctx.ReadValue<Vector2>()));
            input.Player.Attack.performed += ctx => attackEvent.OnNext(Time.fixedTime);
            input.Player.ChangeHand.performed += ctx => changeShapeEvent.OnNext(Unit.Default);
            //input.Player.Rock.performed += ctx => rockEvent.OnNext(Unit.Default);
            //input.Player.Scissors.performed += ctx => scissorsEvent.OnNext(Unit.Default);
            //input.Player.Paper.performed += ctx => paperEvent.OnNext(Unit.Default);

            MoveEvent = moveEvent.Where(_ => isAllowedToMove);
            CancelMoveEvent = cancelMoveEvent;
            ChangeDirectionEvent = changeDirectionEvent.Where(_ => isAllowedToMove);
            AttackEvent = attackEvent.Where(_ => isAllowedToAttack).Select(_ => Unit.Default);
            ChangeShapeEvent = changeShapeEvent.Where(_ => isAllowedToChangeShape);

            //RockEvent = rockEvent.Where(_ => isAllowedRock);
            //ScissorsEvent = scissorsEvent.Where(_ => isAllowedScissors);
            //PaperEvent = paperEvent.Where(_ => isAllowedPaper);
        }

        //private void Update()
        //{
        //    Hand.Shape hand = GetHand();
        //    changeShapeEvent.OnNext(hand);
        //}

        //private Hand.Shape GetHand()
        //{
        //    Keyboard keyboard = Keyboard.current;
        //    List<Key> inputKeys = keyboard.allKeys.Where(x => x.isPressed).Select(x => x.keyCode).ToList();

        //    if (inputKeys.ContainsArray(rockKeys))
        //    {
        //        return Hand.Shape.Rock;
        //    }
        //    else if (inputKeys.ContainsArray(scissorsKeys))
        //    {
        //        return Hand.Shape.Scissors;
        //    }
        //    else
        //    {
        //        return Hand.Shape.Paper;
        //    }
        //}

        public void SetActive(SetPlayerInputActiveCommand command)
        {
            isAllowedToMove = command.move ?? isAllowedToMove;
            isAllowedToAttack = command.attack ?? isAllowedToAttack;
            isAllowedToChangeShape = command.changeShape ?? isAllowedToChangeShape;

            if (!isAllowedToMove) cancelMoveEvent.OnNext(Unit.Default);
        }
        public void SetActiveAll(bool active)
        {
            SetActive(new SetPlayerInputActiveCommand(active, active, active));
        }

        //public void SetMoveKeys(string moveUpPath, string moveDownPath, string moveLeftPath, string moveRightPath)
        //{
        //    input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/upArrow", overridePath = moveUpPath });
        //    input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/downArrow", overridePath = moveDownPath });
        //    input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/leftArrow", overridePath = moveLeftPath });
        //    input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/rightArrow", overridePath = moveRightPath });
        //}
        //public void SetRockKeys(Key[] keys)
        //{
        //    rockKeys = keys;
        //}
        //public void SetScissorsKeys(Key[] keys)
        //{
        //    scissorsKeys = keys;
        //}
    }
}
