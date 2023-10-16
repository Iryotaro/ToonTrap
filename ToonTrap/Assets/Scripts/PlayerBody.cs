using DG.Tweening;
using FTRuntime;
using Ryocatusn.Games;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SwfClipController))]
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField]
        private Camera cameraFinalResult;
        [SerializeField]
        private Animator leftHandAnimator;
        private SwfClipController swfClipController;
        
        [SerializeField]
        private SwfClipAsset attackAnimation;
        [SerializeField]
        private SwfClipAsset idleAnimation;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            swfClipController = GetComponent<SwfClipController>();
        }

        public void ShotLeftHand(Player player, Tilemap firstRoad, Vector2 startPosition, Action finish)
        {
            //だいぶコードが汚いけど重要な場所じゃないし良し
            StartCoroutine(Play());
            IEnumerator Play()
            {
                //敵がプレイヤーに攻撃しないよう距離を開けてる
                player.transform.position = new Vector2(-1000, -1000);
                player.inputMaster.SetActiveAll(false);
                PlaySwfAnimation(attackAnimation);
                //左手を打つタイミング
                yield return new WaitUntil(() => swfClipController.clip.currentFrame >= 49);
                leftHandAnimator.Play("Shot");
                //左手のアニメーションが終わるタイミング
                yield return new WaitForSeconds(1.2f);
                Vector2 leftHandPosition = leftHandAnimator.transform.position;
                Vector2 leftHandViewPortPoint = cameraFinalResult.WorldToViewportPoint(leftHandPosition);
                player.transform.position = gameManager.gameContains.gameCamera.camera.ViewportToWorldPoint(leftHandViewPortPoint);

                Sequence sequence = DOTween.Sequence();
                sequence
                    .SetLink(player.gameObject)
                    .Append(player.transform.DOMove(startPosition, 1))
                    .Join(player.transform.DORotate(new Vector3(0, 0, 360 * 3), 1, RotateMode.FastBeyond360))
                    .SetEase(Ease.InQuart)
                    .OnComplete(() =>
                    {
                        player.tileTransform.ChangeTilemap(new Tilemap[] { firstRoad }, startPosition);
                        player.inputMaster.SetActiveAll(true);

                        //Idleアニメーションに移動
                        swfClipController.loopMode = SwfClipController.LoopModes.Loop;
                        PlaySwfAnimation(idleAnimation);

                        finish();
                    });
            }

            void PlaySwfAnimation(SwfClipAsset swfAnimation)
            {
                swfClipController.clip.clip = swfAnimation;
                swfClipController.Play(true);
            }
        }
    }
}
