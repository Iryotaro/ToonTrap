using Ryocatusn.Conversations;
using Ryocatusn.Games;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class WeatherEffectTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool play = true;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .Subscribe(_ => 
                {
                    if (play) gameManager.gameContains.weatherEffects.Play(); 
                    else gameManager.gameContains.weatherEffects.Stop();
                })
                .AddTo(this);
        }
    }
}