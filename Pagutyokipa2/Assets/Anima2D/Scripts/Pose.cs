using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anima2D
{
    public class Pose : ScriptableObject
    {
        [Serializable]
        public class PoseEntry
        {
            public string path;
            public Vector3 localPosition;
            public Quaternion localRotation;
            public Vector3 localScale;
        }

        [SerializeField]
        List<PoseEntry> m_PoseEntries;
    }
}
