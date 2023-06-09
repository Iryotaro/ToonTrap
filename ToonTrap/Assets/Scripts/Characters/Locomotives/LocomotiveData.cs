using UnityEngine;

namespace Ryocatusn.Characters
{
    [CreateAssetMenu(menuName = "MyAssets/LocomotiveData", fileName = "NewLocomotiveData")]
    public class LocomotiveData : ScriptableObject
    {
        [Range(1, 20)]
        public int numberOfCars = 1;
        [Range(1, 20)]
        public float moveRate = 1;
    }
}
