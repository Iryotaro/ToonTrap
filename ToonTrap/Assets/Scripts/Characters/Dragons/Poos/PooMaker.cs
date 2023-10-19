using Ryocatusn.Audio;
using Ryocatusn.Games;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class PooMaker : MonoBehaviour
    {
        [SerializeField]
        private Poo poo;
        [SerializeField]
        private SE pooSE;

        private SEPlayer sePlayer;

        private void OnEnable()
        {
            sePlayer = new SEPlayer(gameObject);
            Make();
        }

        private void Make()
        {
            Instantiate(poo, transform.position, Quaternion.identity);
            sePlayer.Play(pooSE);
        }
    }
}
