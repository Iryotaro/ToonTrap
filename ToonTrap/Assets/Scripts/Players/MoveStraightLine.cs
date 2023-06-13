using System.Collections.Generic;

namespace Ryocatusn.TileTransforms
{
    public class MoveStraightLine : IMoveDataCreater
    {
        private MoveData moveData;

        public MoveStraightLine(TilePosition position, TileDirection tileDirection, int count)
        {
            if (position == null)
            {
                moveData = null;
                return;
            }

            TilePosition newPosition = position;
            List<TilePosition> data = new List<TilePosition>() { position };

            for (int i = 0; i < count; i++)
            {
                TilePosition aroundTilePosition = newPosition.GetAroundTilePosition(tileDirection);
                if (aroundTilePosition != null)
                {
                    data.Add(aroundTilePosition);
                    newPosition = aroundTilePosition;
                }
                else
                {
                    break;
                }
            }

            if (data.Count == 1)
            {
                moveData = null;
                return;
            }

            moveData = new MoveData(data);
        }

        public MoveData GetData()
        {
            return moveData;
        }

        public bool IsSuccess()
        {
            return moveData != null;
        }
    }
}
