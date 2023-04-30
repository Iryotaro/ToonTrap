using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class Locomotive : JankenBehaviour
    {
        [SerializeField]
        private LocomotiveCar firstCarPrefab;
        [SerializeField]
        private LocomotiveCar carPrefab;

        private List<LocomotiveCar> locomotiveCars = new List<LocomotiveCar>();

        public void SetUp(Hand.Shape shape, Railway railway, LocomotiveData data)
        {
            JankenableObjectCreateCommand createCommand = new JankenableObjectCreateCommand(new Hp(data.numberOfCars), shape);
            Create(createCommand);

            events.VictimLoseEvent
                .Subscribe(_ => BlowAway(locomotiveCars[locomotiveCars.Count - 1]))
                .AddTo(this);

            events.DieEvent
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            StartCoroutine(Start());

            IEnumerator Start()
            {
                for (int i = 0; i < data.numberOfCars; i++)
                {
                    LocomotiveCar newCar = CreateCar(shape, i == 0 ? firstCarPrefab : carPrefab, railway.startPosition.position);

                    Move(newCar, railway, data.moveRate);

                    newCar.BlowAwayEvent
                        .Subscribe(x => BlowAway(x))
                        .AddTo(this);

                    yield return new WaitForSeconds(1 / data.moveRate);
                }
            }
        }

        private LocomotiveCar CreateCar(Hand.Shape shape, LocomotiveCar prefab, Vector2 createPosition)
        {
            LocomotiveCar car = Instantiate(prefab, transform.parent);
            car.transform.position = createPosition;

            locomotiveCars.Add(car);

            AttackableObjectApplicationService attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, shape, new Atk(1));
            AttackableObjectId attackableObjectId = attackableObjectApplicationService.Create(command);

            car.SetUp(attackableObjectId);
            return car;
        }
        private void Move(LocomotiveCar car, Railway railway, float moveRate)
        {
            car.Move(railway, moveRate);
        }
        private void BlowAway(LocomotiveCar locomotiveCar)
        {
            locomotiveCar.BlowAway();
            locomotiveCars.Remove(locomotiveCar);
        }
    }
}
