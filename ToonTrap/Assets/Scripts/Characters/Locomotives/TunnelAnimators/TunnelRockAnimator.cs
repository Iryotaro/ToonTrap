using Ryocatusn.Audio;
using UniRx;
using UnityEngine;
using Ryocatusn.Games;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    public class TunnelRockAnimator : TunnelAnimator
    {
        private Animator animator;

        [SerializeField]
        private SE sunSE;

        [Inject]
        private GameManager gameManager;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void ChangeRateScale(float rateScale)
        {
            animator.speed = rateScale;
        }

        public void HandleCreateLocomotiveFromAnimation()
        {
            createLocomotiveEvent.OnNext(Unit.Default);
        }
        public void HandlePlaySunSEFromAnimation()
        {
            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);
            sePlayer.Play(sunSE);
        }
    }
}
