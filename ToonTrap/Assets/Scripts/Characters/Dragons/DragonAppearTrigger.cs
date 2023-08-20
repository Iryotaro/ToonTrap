using UnityEngine;
using UniRx;
using System;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class DragonAppearTrigger : MonoBehaviour
    {
        [SerializeField]
        private Dragon dragon;
        [SerializeField]
        private AppearType appearType;

        public enum AppearType
        {
            FirstAppearance,
            Appear
        }

        private void Start()
        {
            Action firstAppearanceAction;
            if (appearType == AppearType.FirstAppearance)
            {
                dragon.FirstAppearance(out firstAppearanceAction);
            }
            else
            {
                return;
            }

            TileTransformTrigger trigger = GetComponent<TileTransformTrigger>();

            trigger.OnHitPlayerEvent
                .First()
                .Subscribe(_ => 
                {
                    switch (appearType)
                    {
                        case AppearType.FirstAppearance:
                            firstAppearanceAction();
                            break;
                        case AppearType.Appear:
                            dragon.Appear();
                            break;
                    }
                })
                .AddTo(this);
        }
    }
}
