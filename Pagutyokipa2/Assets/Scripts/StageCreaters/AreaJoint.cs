using System;
using Ryocatusn.TileTransforms;
using System.Linq;

namespace Ryocatusn.StageCreaters
{
    [Serializable]
    public class AreaJoint
    {
        public TileTransform transform;
        public TileDirection.Direction[] allowDirections;

        public TilePosition GetPositoin()
        {
            return transform.tilePosition.Get();
        }
        public bool IsAllowedToConnect(TileDirection.Direction direction)
        {
            return allowDirections.Contains(direction);
        }
    }
}
