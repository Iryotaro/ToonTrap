using System;
using UnityEngine;

namespace Ryocatusn.Lights
{
    [Serializable]
    public class LightContains
    {
        [SerializeField]
        private GlobalLight globalLight;
        [SerializeField]
        private SpotLight spotLight;
    }
}
