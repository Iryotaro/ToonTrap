using Ryocatusn.Characters;
using Ryocatusn.Games;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Ryocatusn
{
    public class StageStartPresenter : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        public void Play(Vector2 startPosition, Tilemap firstRoad, Action finish)
        {
            Player player = gameManager.gameContains.player;
            PlayerBody playerBody = gameManager.gameContains.playerBody;

            playerBody.ShotLeftHand(player, firstRoad, startPosition, finish);
        }
    }
}
