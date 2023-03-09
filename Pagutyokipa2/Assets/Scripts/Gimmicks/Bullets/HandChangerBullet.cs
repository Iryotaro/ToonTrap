using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Ryocatusn.Ryoseqs;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Collision2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class HandChangerBullet : MonoBehaviour
    {
        private JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();
        private SEPlayer sePlayer;
        private (Transform transform, IReceiveAttack receiveAttack) target;
        private float speed = 3;

        [SerializeField]
        private SE handChangerSE;

        public void Set((Transform transform, IReceiveAttack receiveAttack) target)
        {
            this.target = target;
            sePlayer = new SEPlayer(gameObject);

            float rot = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, rot);
        }

        private void Update()
        {
            Move();

            if (Vector3.Distance(target.transform.position, transform.position) < 0.5f)
            {
                ChangeHand(target.receiveAttack.id);
                Destroy(gameObject);
            }
        }

        private void Move()
        {
            float rot = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, rot);
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
            speed += 2 * Time.deltaTime;
        }
        private void ChangeHand(JankenableObjectId id)
        {
            GameContains gameContains = StageManager.activeStage.gameContains.Get();
            if (gameContains == null) return;

            Ryoseq ryoseq = new Ryoseq();

            ISequence sequence = ryoseq.Create();
            sequence
                .Connect(new SequenceCommand(_ =>
                {
                    sePlayer.Play(handChangerSE);
                    Hand.Shape[] shapes = Enum.GetValues(typeof(Hand.Shape)).OfType<Hand.Shape>().Where(shpae => !shpae.Equals(jankenableObjectApplicationService.Get(id).shape)).ToArray();
                    Hand.Shape changeShape = shapes[UnityEngine.Random.Range(0, shapes.Length)];
                    jankenableObjectApplicationService.ChangeShape(id, changeShape);
                    gameContains.player.inputMaster.SetActive(new SetPlayerInputActiveCommand(rock: false, scissors: false, paper: false));

                    gameContains.playerButtonMappingUI.transform
                    .DOShakePosition(2, 30, 10, 1, false, true)
                    .SetLink(gameContains.playerButtonMappingUI.gameObject);
                }))
                .ConnectWait(new SequenceWaitForSeconds(2))
                .Connect(new SequenceCommand(_ =>
                {
                    gameContains.player.inputMaster.SetActive(new SetPlayerInputActiveCommand(rock: true, scissors: true, paper: true));
                    ryoseq.Delete();
                }));

            ryoseq.MoveNext();
        }
    }
}
