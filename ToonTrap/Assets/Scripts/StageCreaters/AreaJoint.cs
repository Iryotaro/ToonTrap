using Ryocatusn.TileTransforms;
using System;
using System.Linq;

namespace Ryocatusn.StageCreaters
{
    public abstract class AreaJoint
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
