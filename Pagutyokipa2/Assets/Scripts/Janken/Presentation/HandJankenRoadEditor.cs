using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ryocatusn.Janken
{
    [ExecuteAlways()]
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(JankenRoad))]
    public class HandJankenRoadEditor : MonoBehaviour
    {
        private void Start()
        {
            JankenRoad jankenRoad = GetComponent<JankenRoad>();
            ChangeTile(jankenRoad.GetHandShape(), jankenRoad.IsCrack() ? jankenRoad.GetCrackHandTiles() : jankenRoad.GetHandTiles());
        }
        private void Update()
        {
            if (Application.isPlaying) return;

            JankenRoad jankenRoad = GetComponent<JankenRoad>();
            ChangeTile(jankenRoad.GetHandShape(), jankenRoad.IsCrack() ? jankenRoad.GetCrackHandTiles() : jankenRoad.GetHandTiles());
        }
        
        private void ChangeTile(Hand.Shape shape, HandTiles tiles)
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                tilemap.SetTile(pos, shape switch
                {
                    Hand.Shape.Rock => tiles.rockTile,
                    Hand.Shape.Scissors => tiles.scissorsTile,
                    Hand.Shape.Paper => tiles.paperTile,
                    _ => null
                });
            }
        }
    }
}
