using Ryocatusn.Games;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(Tilemap))]
    public class Road : MonoBehaviour
    {
        [Inject]
        private DiContainer diContainer;
        [Inject]
        private StageManager stageManager;
        [Inject]
        private GameManager gameManager;

        [SerializeField]
        private bool m_appear = false;
        [SerializeField]
        private RoadAnime roadAnime;

        private Dictionary<Vector3Int, Sprite> spriteDictionary = new Dictionary<Vector3Int, Sprite>();

        private bool appear;

        [System.NonSerialized]
        public Tilemap tilemap;
        private TilemapRenderer tilemapRenderer;

        private void Start()
        {
            appear = m_appear;

            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();

            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(position)) continue;
                spriteDictionary.Add(position, tilemap.GetSprite(position));
            }

            tilemapRenderer.enabled = appear;

            if (appear)
            {
                stageManager.SetupStageEvent
                    .Subscribe(gameContains =>
                    {
                        stageManager.AddRoad(tilemap);
                    }).AddTo(this);
            }

            RoadManager.instance.Save(this);
        }
        private void OnDestroy()
        {
            RoadManager.instance.Delete(this);
        }

        public void Appear()
        {
            if (appear) return;
            appear = true;

            stageManager.SetupStageEvent
                .Subscribe(_ =>
                {
                    stageManager.AddRoad(tilemap);
                    StartCoroutine(AppearRoadCoroutine(GetTiles(tilemap, gameManager.gameContains.player)));
                }).AddTo(this);
        }

        private IEnumerator AppearRoadCoroutine(List<Vector3Int> positions)
        {
            List<RoadAnime> roadAnimes = new List<RoadAnime>();
            foreach (Vector3Int position in positions)
            {
                roadAnimes.Add(AppearTile(tilemap.CellToWorld(position), roadAnime));
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (RoadAnime roadAnime in roadAnimes) Destroy(roadAnime.gameObject);
            tilemapRenderer.enabled = true;
        }

        private List<Vector3Int> GetTiles(Tilemap tilemap, Player player)
        {
            List<Vector3Int> positions = new List<Vector3Int>();
            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(position)) continue;
                positions.Add(position);
            }

            positions = positions.OrderBy(x => Vector2.Distance(tilemap.CellToWorld(x), player.transform.position)).ToList();

            return positions;
        }
        private RoadAnime AppearTile(Vector3 position, RoadAnime roadAnime)
        {
            RoadAnime newRoadAnime = Instantiate(roadAnime, transform);
            newRoadAnime.transform.position = position + roadAnime.transform.lossyScale / 2;
            newRoadAnime.ChangeSprite(spriteDictionary[tilemap.WorldToCell(position)]);
            diContainer.InjectGameObject(newRoadAnime.gameObject);
            return newRoadAnime;
        }
    }
}
