using Ryocatusn.Audio;
using UnityEngine;
using Ryocatusn.Games;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoadAnime : MonoBehaviour
    {
        [SerializeField]
        private SE se;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            new SEPlayer(gameObject, gameManager.gameContains.gameCamera).Play(se);
        }
        public void ChangeSprite(Sprite sprite)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
