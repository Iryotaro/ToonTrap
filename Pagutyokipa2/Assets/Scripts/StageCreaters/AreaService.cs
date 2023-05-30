using Ryocatusn.TileTransforms;

namespace Ryocatusn.StageCreaters
{
    public class AreaService
    {
        public bool IsAllowedToConnect(Area prevArea, Area nextArea)
        {
            foreach (TileDirection.Direction direction in nextArea.startJoint.allowDirections)
            {
                if (prevArea.endJoint.IsAllowedToConnect(direction)) return true;
                else continue;
            }
            return false;
        }
    }
}
