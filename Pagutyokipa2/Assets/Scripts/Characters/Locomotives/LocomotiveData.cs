using UnityEngine;

namespace Ryocatusn.Characters 
{
    [CreateAssetMenu(menuName = "MyAssets/LocomotiveData", fileName = "NewLocomotiveData")]
    public class LocomotiveData : ScriptableObject
    {
        [Range(1, 5)]
        public int numberOfCars = 1;
        [Range(0.5f, 10)]
        public float moveRate = 0.5f;
    }
}
