using Ryocatusn.Games;
using System;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Lights
{
    [RequireComponent(typeof(SpotLight))]
    public class PlayerLight : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        [NonSerialized]
        public SpotLight spotLight;

        [SerializeField]
        private Transform extraPosition;

        private void Start()
        {
            spotLight = GetComponent<SpotLight>();
        }
        private void Update()
        {
            Player player = gameManager.gameContains.player;
            Vector2 playerWorldPositionOnFinalResult = gameManager.GetWorldPositoinOnFinalResult(player.transform.position);
            transform.position = playerWorldPositionOnFinalResult;

            spotLight.TurnOnExtra(extraPosition.position);
        }
    }
}
