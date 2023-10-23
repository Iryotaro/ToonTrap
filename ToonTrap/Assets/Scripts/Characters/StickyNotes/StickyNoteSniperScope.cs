using UnityEngine;
using System;
using Ryocatusn.Janken.AttackableObjects;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(StickyNoteSniperScopeView))]
    public class StickyNoteSniperScope : AttackBehaviour
    {
        [SerializeField]
        private int chaseTime;

        private IReceiveAttack targetReceiveAttack;
        private List<IReceiveAttack> reachReceiveAttacks = new List<IReceiveAttack>();

        private Subject<Unit> shotSubject = new Subject<Unit>();

        public IObservable<Unit> ShotSubject => shotSubject;

        public void Setup(AttackableObjectId id, IReceiveAttack receiveAttack)
        {
            transform.parent = null;

            StickyNoteSniperScopeView view = GetComponent<StickyNoteSniperScopeView>();

            targetReceiveAttack = receiveAttack;
            SetId(id, true);

            GameCamera gameCamera = gameManager.gameContains.gameCamera;

            Vector2 randomViewport = new Vector2(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
            Vector2 randomWorldPosition = gameCamera.camera.ViewportToWorldPoint(randomViewport);
            transform.position = randomWorldPosition;

            view.SetUp(id, chaseTime);

            this.OnTriggerEnter2DAsObservable()
                .Select(x =>
                {
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack)) return receiveAttack;
                    else return null;
                })
                .Where(x => x != null)
                .Subscribe(x => reachReceiveAttacks.Add(x));

            this.OnTriggerExitAsObservable()
                .Select(x =>
                {
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack)) return receiveAttack;
                    else return null;
                })
                .Where(x => x != null)
                .Subscribe(x => reachReceiveAttacks.Remove(x));

            this.UpdateAsObservable()
                .Subscribe(_ => ChaseToPlayer());

            view.ShotSubject
                .Subscribe(_ => Shot())
                .AddTo(this);
        }
        private void OnDestroy()
        {
            shotSubject.Dispose();
        }

        private void ChaseToPlayer()
        {
            Vector2 playerPosition = gameManager.gameContains.player.transform.position;
            Vector2 currentVelocity = Vector2.zero;
            Vector2 worldPosition = Vector2.SmoothDamp(transform.position, playerPosition, ref currentVelocity, 0.1f);
            transform.position = worldPosition;
        }

        private void Shot()
        {
            if (reachReceiveAttacks.Contains(targetReceiveAttack)) Attack(targetReceiveAttack);
            shotSubject.OnNext(Unit.Default);
        }
    }
}
