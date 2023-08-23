using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    public class Poo : MonoBehaviour
    {
        private void Start()
        {
            Animator animator = GetComponent<Animator>();
            animator.Play("Poo" + Random.Range(1, 3 + 1));
        }

        public void OnDestroyEventFromAnimation()
        {
            Destroy(gameObject);
        }
    }
}
