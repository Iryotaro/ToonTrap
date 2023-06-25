using Ryocatusn.Janken;
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
        [SerializeField]
        private UnityEngine.UI.Image testImage;
        private Hand.Shape testChangeShape;

        private InputMaster input;

        public bool isAllowedMove { get; private set; } = true;
        public bool isAllowedAttack { get; private set; } = true;
        public bool isAllowedRock { get; private set; } = true;
        public bool isAllowedScissors { get; private set; } = true;
        public bool isAllowedPaper { get; private set; } = true;

        [NonSerialized]
        public Key[] rockKeys = new Key[] { Key.R, Key.E, Key.W, Key.A };
        [NonSerialized]
        public Key[] scissorsKeys = new Key[] { Key.W, Key.A };

        private Subject<Unit> moveEvent = new Subject<Unit>();
        private Subject<Unit> specialEvent = new Subject<Unit>();
        private Subject<Unit> cancelMoveEvent = new Subject<Unit>();
        private Subject<TileDirection> changeDirectionEvent = new Subject<TileDirection>();
        private Subject<float> attackEvent = new Subject<float>();
        private Subject<Hand.Shape> changeShapeEvent = new Subject<Hand.Shape>();

        public IObservable<Unit> MoveEvent { get; private set; }
        public IObservable<Unit> SpecialEvent { get; private set; }
        public IObservable<Unit> CancelMoveEvent { get; private set; }
        public IObservable<TileDirection> ChangeDirectionEvent { get; private set; }
        public IObservable<Unit> AttackEvent { get; private set; }
        public IObservable<Hand.Shape> ChangeShapeEvent { get; private set; }

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
            input.Player.Special.performed += ctx => specialEvent.OnNext(Unit.Default);
            input.Player.Attack.performed += ctx => attackEvent.OnNext(Time.fixedTime);
            input.Player.ChangeHand.performed += ctx => changeShapeEvent.OnNext(GetChangeShape());
            //input.Player.Rock.performed += ctx => rockEvent.OnNext(Unit.Default);
            //input.Player.Scissors.performed += ctx => scissorsEvent.OnNext(Unit.Default);
            //input.Player.Paper.performed += ctx => paperEvent.OnNext(Unit.Default);

            MoveEvent = moveEvent.Where(_ => isAllowedMove);
            SpecialEvent = specialEvent.Where(_ => isAllowedMove);
            CancelMoveEvent = cancelMoveEvent;
            ChangeDirectionEvent = changeDirectionEvent.Where(_ => isAllowedMove);
            AttackEvent = attackEvent.Where(_ => isAllowedAttack).Select(_ => Unit.Default);
            ChangeShapeEvent = changeShapeEvent
                .Where(x =>
                {
                    if (x == Hand.Shape.Rock) return isAllowedRock;
                    if (x == Hand.Shape.Scissors) return isAllowedScissors;
                    if (x == Hand.Shape.Paper) return isAllowedPaper;
                    return false;
                });

            //RockEvent = rockEvent.Where(_ => isAllowedRock);
            //ScissorsEvent = scissorsEvent.Where(_ => isAllowedScissors);
            //PaperEvent = paperEvent.Where(_ => isAllowedPaper);

            StartCoroutine(test());
            System.Collections.IEnumerator test()
            {
                while (true)
                {
                    testChangeShape = Hand.GetNextShape(testChangeShape);
                    testImage.color = testChangeShape switch
                    {
                        Hand.Shape.Rock => Color.red,
                        Hand.Shape.Scissors => Color.yellow,
                        Hand.Shape.Paper => Color.blue,
                        _ => Color.white
                    };
                    yield return new WaitForSeconds(1);
                }
            }
        }

        private Hand.Shape GetChangeShape()
        {
            return testChangeShape;
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
            isAllowedMove = command.move ?? isAllowedMove;
            isAllowedAttack = command.attack ?? isAllowedAttack;
            isAllowedRock = command.rock ?? isAllowedRock;
            isAllowedScissors = command.scissors ?? isAllowedScissors;
            isAllowedPaper = command.paper ?? isAllowedPaper;

            if (!isAllowedMove) cancelMoveEvent.OnNext(Unit.Default);
        }
        public void SetActiveAll(bool active)
        {
            SetActive(new SetPlayerInputActiveCommand(active, active, active, active, active));
        }

        public void SetMoveKeys(string moveUpPath, string moveDownPath, string moveLeftPath, string moveRightPath)
        {
            input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/upArrow", overridePath = moveUpPath });
            input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/downArrow", overridePath = moveDownPath });
            input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/leftArrow", overridePath = moveLeftPath });
            input.Player.Move.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/rightArrow", overridePath = moveRightPath });
        }
        public void SetRockKeys(Key[] keys)
        {
            rockKeys = keys;
        }
        public void SetScissorsKeys(Key[] keys)
        {
            scissorsKeys = keys;
        }
    }
}
