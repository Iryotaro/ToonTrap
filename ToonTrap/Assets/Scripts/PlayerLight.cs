using Ryocatusn.Games;
using System;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(SpotLight))]
    public class PlayerLight : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        [NonSerialized]
        public SpotLight spotLight;

        [SerializeField]
        private Transform extraPosition1;
        [SerializeField]
        private Transform extraPosition2;

        private void Start()
        {
            spotLight = GetComponent<SpotLight>();
        }
        private void Update()
        {
            Player player = gameManager.gameContains.player;
            Vector2 playerWorldPositionOnFinalResult = gameManager.GetWorldPositoinOnFinalResult(player.transform.position);
            transform.position = playerWorldPositionOnFinalResult;

            spotLight.TurnOnExtra(extraPosition1.position, extraPosition2.position);
        }
    }
}
