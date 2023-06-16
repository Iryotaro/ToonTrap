using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ryocatusn.Audio
{
    public class SEPlayer
    {
        private GameObject owner { get; }
        private Renderer renderer { get; }
        private Tilemap tilemap { get; }
        private List<SE> seList = new List<SE>();
        private bool dontDestroyOnLoad = false;

        public SEPlayer(GameObject owner)
        {
            this.owner = owner;

            this.owner.OnDestroyAsObservable()
                .Subscribe(_ => Delete());
        }
        public SEPlayer(GameObject owner, Renderer renderer)
        {
            this.owner = owner;
            this.renderer = renderer;

            this.owner.OnDestroyAsObservable()
                .Subscribe(_ => Delete());
        }
        public SEPlayer(GameObject owner, Tilemap tilemap)
        {
            this.owner = owner;
            this.tilemap = tilemap;

            this.owner.OnDestroyAsObservable()
                .Subscribe(_ => Delete());
        }

        private AudioSource AddSE(SE se)
        {
            seList.Add(se);

            GameObject sePlayer = new GameObject();
            sePlayer.name = $"{owner.name} SEPlayer";

            if (dontDestroyOnLoad)
            {
                MonoBehaviour.DontDestroyOnLoad(sePlayer);
            }
            else
            {
                GameObject parent = GameObject.Find("SE");
                if (parent == null) parent = new GameObject() { name = "SE" };
                sePlayer.transform.parent = parent.transform;
            }

            AudioSource audioSource = sePlayer.AddComponent<AudioSource>();
            audioSource.clip = se.audioClip;
            audioSource.pitch = se.pitch;
            audioSource.volume = se.volume;
            audioSource.loop = se.loop;
            se.audioSource = audioSource;

            return audioSource;
        }

        public void Play(SE se)
        {
            if (!IsAllowedPlay(se)) return;

            AudioSource audioSource = null;

            seList.RemoveAll(x => x.audioSource == null);

            SE foundSE = seList.Find(x => x.Equals(se));
            if (foundSE != null) audioSource = foundSE.audioSource;
            else audioSource = AddSE(se);

            if (!audioSource.loop) audioSource.PlayOneShot(se.audioClip);
            else audioSource.Play();
        }
        public void Stop(SE se)
        {
            AudioSource audioSource = null;

            SE foundSE = seList.Find(x => x.Equals(se));
            if (foundSE != null) audioSource = foundSE.audioSource;
            else return;

            audioSource.Stop();
        }
        public void DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;
        }

        private bool IsAllowedPlay(SE se)
        {
            if (!se.onlyVisible) return true;
            if (owner == null) return false;

            if (renderer != null)
            {
                if (renderer.isVisible) return true;
                else return false;
            }

            Vector3 position = owner.transform.position;
            if (tilemap != null)
            {
                foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
                {
                    if (!tilemap.HasTile(pos)) continue;
                    position = tilemap.CellToWorld(pos) + Vector3.Scale(tilemap.cellSize, tilemap.transform.lossyScale) / 2;
                }
            }

            Camera mainCamera = Camera.main;
            Vector2 screenPoint = (Vector2)mainCamera.WorldToViewportPoint(position);
            if (screenPoint.x >= 0 && screenPoint.x <= 1 &&
                screenPoint.y >= 0 && screenPoint.y <= 1) return true;
            return false;
        }

        private void Delete()
        {
            WaitAndDelete().ToUniTask().Forget();

            IEnumerator WaitAndDelete()
            {
                yield return new WaitUntil(() => IsAllowedToDelete());

                Delete();
            }

            bool IsAllowedToDelete()
            {
                SE playingSE = seList.Find(x =>
                {
                    if (x.audioSource == null) return false;
                    return !x.audioSource.isPlaying;
                });

                return playingSE == null;
            }
            void Delete()
            {
                foreach (SE se in seList)
                {
                    if (se.audioSource == null) continue;
                    GameObject.Destroy(se.audioSource.gameObject);
                }
            }
        }
    }
}
