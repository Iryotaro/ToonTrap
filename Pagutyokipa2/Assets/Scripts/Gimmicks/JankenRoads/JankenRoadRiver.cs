using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ryocatusn.TileTransforms;
using Ryocatusn.Janken;

namespace Ryocatusn
{
    [RequireComponent(typeof(Tilemap))]
    public class JankenRoadRiver : MonoBehaviour
    {
        [SerializeField]
        private JankenRoad jankenRoad;
        [SerializeField, Min(0.2f)]
        private float createTimeSpan = 1;
        [SerializeField]
        private TileTransform startPosition;
        [SerializeField]
        private TileTransform endPosition;
        [SerializeField, Min(0)]
        private float moveRate;

        private Tilemap tilemap;

        private void Start()
        {
            tilemap = GetComponent<Tilemap>();

            InvokeRepeating(nameof(Create), 0, createTimeSpan);
        }

        private void Create()
        {
            Vector3 startWorldPosition = startPosition.tilePosition.Get().GetWorldPosition();
            Vector3 endWorldPosition = endPosition.tilePosition.Get().GetWorldPosition();

            TileTransform tileTransform = new GameObject().AddComponent<TileTransform>();
            tileTransform.gameObject.name = "JankenRoadTilemap";
            tileTransform.transform.parent = tilemap.transform;
            tileTransform.transform.position = startWorldPosition;
            tileTransform.ChangeTilemap(new Tilemap[] { tilemap }, startWorldPosition);

            JankenRoad newJankenRoad = Instantiate(jankenRoad, Vector3.zero, Quaternion.identity);
            newJankenRoad.transform.parent = tileTransform.transform;
            Tilemap roadTilemap = newJankenRoad.GetComponent<Tilemap>();
            roadTilemap.SetTile(roadTilemap.WorldToCell(startWorldPosition), tilemap.GetTile(tilemap.WorldToCell(startWorldPosition)));

            Hand.Shape shape = Random.Range(1, 3 + 1) switch { 1 => Hand.Shape.Rock, 2 => Hand.Shape.Scissors, 3 => Hand.Shape.Paper, _ => default };
            newJankenRoad.shape = shape;

            MoveAStar aSter = new MoveAStar(startWorldPosition, endWorldPosition, new List<Tilemap>() { tilemap });
            tileTransform.SetMovement(aSter, new MoveRate(moveRate));

            tileTransform.movement.Get().CompleteEvent.Subscribe(_ =>
            {
                newJankenRoad.Break(true);
                //汚いコード
                Destroy(tileTransform.gameObject, 3);
            }).AddTo(this);
        }
    }
}
