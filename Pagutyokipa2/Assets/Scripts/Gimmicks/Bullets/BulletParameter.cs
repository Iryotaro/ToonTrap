using System;
using UnityEngine;

namespace Ryocatusn
{
    [Serializable]
    public class BulletParameter
    {
        [Range(0.1f, 30)]
        public float speed = 5;
        [Range(0.3f, 15)]
        public float timeToDelete = 5;
    }
}
