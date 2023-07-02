using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Audio
{
    public class SEPlayer
    {
        private GameObject owner { get; }
        private List<SE> seList = new List<SE>();
        private bool dontDestroyOnLoad = false;
        private GameCamera gameCamera;

        public SEPlayer(GameObject owner, GameCamera gameCamera)
        {
            this.owner = owner;
            this.gameCamera = gameCamera;

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

            return !gameCamera.IsOutSideOfCamera(owner);
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
