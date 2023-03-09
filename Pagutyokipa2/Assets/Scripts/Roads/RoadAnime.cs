using UnityEngine;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoadAnime : MonoBehaviour
    {
        [SerializeField]
        private SE se;

        private void Start()
        {
            new SEPlayer(gameObject).Play(se);
        }
        public void ChangeSprite(Sprite sprite)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
