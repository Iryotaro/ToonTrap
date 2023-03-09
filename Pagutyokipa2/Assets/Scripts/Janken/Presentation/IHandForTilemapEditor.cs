using UnityEngine.Tilemaps;

namespace Ryocatusn.Janken
{
    public interface IHandForTilemapEditor
    {
        Hand.Shape GetHandShape();
        HandTiles GetHandTiles();
    }
}
