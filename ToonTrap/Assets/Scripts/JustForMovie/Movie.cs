using Ryocatusn.Characters;
using System.Collections;
using UnityEngine;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    public class Movie : MonoBehaviour
    {
        [SerializeField]
        private Dragon dragon1;
        [SerializeField]
        private Dragon dragon2;
        [SerializeField]
        private SE damageSe;

        private void Start()
        {
            StartCoroutine(A());

            IEnumerator A()
            {
                yield return new WaitForSeconds(8);
                dragon1.dragonAnimator.PlayAnimations(new DragonAnimator.AnimationType[] { DragonAnimator.AnimationType.Appear, DragonAnimator.AnimationType.FirstAppearance, DragonAnimator.AnimationType.Provocation });
                yield return new WaitForSeconds(10);
                dragon1.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Attack1);
                yield return new WaitForSeconds(2);
                dragon1.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Disappear);

                yield return new WaitForSeconds(4);
                dragon2.dragonAnimator.PlayAnimations(new DragonAnimator.AnimationType[] { DragonAnimator.AnimationType.Appear, DragonAnimator.AnimationType.Attack1 });
                yield return new WaitForSeconds(4);
                new SEPlayer(gameObject).Play(damageSe);
                dragon2.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Damage);
                yield return new WaitForSeconds(2);
                dragon2.dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Disappear);
            }
        }
    }
}
