using System.Collections.Generic;
using UnityEngine;

namespace Anima2D
{
    public class RectManipulatorData : IRectManipulatorData
    {
        List<Vector3> m_NormalizedVertices = new List<Vector3>();

        public List<Vector3> normalizedVertices
        {
            get
            {
                return m_NormalizedVertices;
            }
            set
            {
                m_NormalizedVertices = value;
            }
        }
    }
}
