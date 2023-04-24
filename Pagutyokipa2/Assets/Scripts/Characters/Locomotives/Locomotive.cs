using Cysharp.Threading.Tasks;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using System.Collections;
using UnityEngine;
using UniRx;

namespace Ryocatusn.Characters
{
    public class Locomotive : JankenBehaviour
    {
        [SerializeField]
        private LocomotiveCar firstCarPrefab;
        [SerializeField]
        private LocomotiveCar carPrefab;

        private AttackableObjectId attackId;

        public void SetUp(Hand.Shape shape, Railway railway, LocomotiveData data)
        {
            JankenableObjectCreateCommand createCommand = new JankenableObjectCreateCommand(new Hp(1), shape);
            Create(createCommand);

            AttackableObjectCreateCommand attackCommand = new AttackableObjectCreateCommand(id, shape, new Atk(1));
            
            events.AttackTriggerEvent
                .Subscribe(x => attackId = x.id)
                .AddTo(this);
            
            AttackTrigger(attackCommand);

            StartCoroutine(Start());

            IEnumerator Start()
            {
                yield return new WaitUntil(() => attackId != null);

                for (int i = 0; i < data.numberOfCars; i++)
                {
                    LocomotiveCar newCar = CreateCar(i == 0 ? firstCarPrefab : carPrefab, railway.startPosition.position);

                    Move(newCar, railway, data.moveRate);

                    yield return new WaitForSeconds(1 / data.moveRate);
                }
            }
        }

        private LocomotiveCar CreateCar(LocomotiveCar prefab, Vector2 createPosition)
        {
            LocomotiveCar car = Instantiate(prefab, transform);
            car.transform.position = createPosition;
            car.SetUp(attackId);
            return car;
        }
        private void Move(LocomotiveCar car, Railway railway, float moveRate)
        {
            car.Move(railway, moveRate);
        }
    }
}
