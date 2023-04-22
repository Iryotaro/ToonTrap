using UnityEngine;

namespace Ryocatusn.Characters 
{
    [CreateAssetMenu(menuName = "MyAssets/LocomotiveData", fileName = "NewLocomotiveData")]
    public class LocomotiveData : ScriptableObject
    {
        [Range(0, 5)]
        public int numberOfCars = 0;
        [Range(0.5f, 5)]
        public float moveRate = 0.5f;
    }
}
