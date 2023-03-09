using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ryocatusn.Janken
{
    [ExecuteAlways()]
    [RequireComponent(typeof(IHandForTilemapEditor))]
    [RequireComponent(typeof(Tilemap))]
    public class HandTilemapEditor : MonoBehaviour
    {
        private void Awake()
        {
            ChangeTile();
        }
        private void Update()
        {
            if (Application.isPlaying) return;

            ChangeTile();
        }

        private void ChangeTile()
        {
            IHandForTilemapEditor hand = GetComponent<IHandForTilemapEditor>();
            Tilemap tilemap = GetComponent<Tilemap>();

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                tilemap.SetTile(pos, hand.GetHandShape() switch
                {
                    Hand.Shape.Rock => hand.GetHandTiles().rockTile,
                    Hand.Shape.Scissors => hand.GetHandTiles().scissorsTile,
                    Hand.Shape.Paper => hand.GetHandTiles().paperTile,
                    _ => null
                });
            }
        }
    }
}
