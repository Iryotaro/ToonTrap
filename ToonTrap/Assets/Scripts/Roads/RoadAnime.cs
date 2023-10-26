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

        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private AnimeType type;

        public enum AnimeType
        {
            Appear,
            Disappear
        }

        [Inject]
        private GameManager gameManager;

        public void Setup(AnimeType type)
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (type == AnimeType.Appear) spriteRenderer.enabled = false;

            this.type = type;
        }
        public void Play()
        {
            if (type == AnimeType.Appear)
            {
                spriteRenderer.enabled = true;
                animator.SetBool("Appear", true);
            }
            else
            {
                animator.SetBool("Disappear", true);
            }

            new SEPlayer(gameObject, gameManager.gameContains.gameCamera).Play(se);
        }
        public void ChangeSprite(Sprite sprite)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
