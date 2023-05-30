using Ryocatusn.StageCreaters;
using Ryocatusn.TileTransforms;
using System.Collections.Generic;
using System.Linq;

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
    public TilePosition GetNewNextAreaStartPosition(Area prevArea, Area nextArea)
    {
        AreaJoint prevEndJoint = prevArea.endJoint;
        AreaJoint nextStartJoint = nextArea.startJoint;

        List<TileDirection.Direction> candidateDirections = prevEndJoint.allowDirections.ToList().FindAll(nextStartJoint.allowDirections.ToList().Contains);
        TileDirection.Direction direction = candidateDirections[UnityEngine.Random.Range(0, candidateDirections.Count - 1)];

        return prevEndJoint.GetPositoin().GetAroundTilePosition(new TileDirection(direction));
    }
}
