using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class Locomotive : JankenBehaviour
    {
        [SerializeField]
        private LocomotiveCar firstCarPrefab;
        [SerializeField]
        private LocomotiveCar carPrefab;

        private LocomotiveCar firstCar;
        private List<LocomotiveCar> cars = new List<LocomotiveCar>();

        public void SetUp(Hand.Shape shape, Railway railway, LocomotiveData data)
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape);
            Create(command);
            CreateCars(railway, data.numberOfCars);
            Move(railway, data.moveRate);
        }

        private void CreateCars(Railway railway, int numberOfCars)
        {
            firstCar = Instantiate(firstCarPrefab, transform);
            firstCar.transform.position = railway.startPosition.position;
            firstCar.SetUp(GetData().shape);

            for (int i = 0; i < numberOfCars; i++)
            {
                LocomotiveCar newCar = Instantiate(carPrefab, transform);
                newCar.transform.position = railway.startPosition.position;
                cars.Add(newCar);
                newCar.SetUp(GetData().shape);
            }
        }
        private void Move(Railway railway, float moveRate)
        {
            firstCar.Move(railway, moveRate);
        }
    }
}
