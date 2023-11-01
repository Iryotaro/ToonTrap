using Ryocatusn.Characters;
using Ryocatusn.Games;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Photographers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace Ryocatusn
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickyNoteView : MonoBehaviour, IPhotographerSubject
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private ParticleSystem damageEffect;
        [SerializeField]
        private GameObject returnBullet;

        private Vector2 viewport;
        private StickyNoteSniperScope sniperScope;

        private Animator animator;
        private Rigidbody2D rigid;

        private bool finishToAttack = false;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        public int priority { get; } = 10;
        public int photographerCameraSize { get; } = 3;
        public Subject<Unit> showOnPhotographerEvent { get; }

        public void Setup(JankenableObjectEvents events, Vector2 viewport, StickyNoteSniperScope sniperScope)
        {
            this.viewport = viewport;
            this.sniperScope = sniperScope;

            events.WinEvent
                .FirstOrDefault()
                .Subscribe(_ => MoveUp())
                .AddTo(this);

            events.DrawEvent
                .FirstOrDefault()
                .Subscribe(_ => MoveUp())
                .AddTo(this);

            events.LoseEvent
                .FirstOrDefault()
                .Subscribe(_ => GetDamage())
                .AddTo(this);

            sniperScope.ShotEvent
                .Subscribe(_ => animator.SetTrigger("Attack"))
                .AddTo(this);

            sniperScope.HitEvent
                .Subscribe(_ => finishToAttack = true)
                .AddTo(this);

            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();

            photographerSubjectManager.Save(this);
        }
        private void OnDestroy()
        {
            photographerSubjectManager.Delete(this);
        }

        private void GetDamage()
        {
            GameObject returnBullet = Instantiate(this.returnBullet, sniperScope.transform.position, Quaternion.identity);

            returnBullet.transform
                .DOMove(transform.position, 0.6f)
                .SetLink(gameObject)
                .SetEase(Ease.InQuart)
                .OnComplete(() => 
                {
                    damageEffect.Play();
                    animator.SetTrigger("Damage");
                    Destroy(returnBullet);
                });

            this.OnDestroyAsObservable()
                .Where(_=> returnBullet != null)
                .Subscribe(_ => Destroy(returnBullet));
        }

        private void Update()
        {
            if (!finishToAttack) Move();
            if (IsAllowedToDelete()) Delete();
        }

        private void MoveUp()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => rigid.AddForce(Vector2.up * 3));
        }
        private void Move()
        {
            Camera cam = gameManager.gameContains.gameCamera.camera;
            Vector3 target = cam.ViewportToWorldPoint(new Vector2(viewport.x, viewport.y));
            target = new Vector3(target.x, target.y, 0);
            Vector3 currentVelocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, 1 / speed);
        }

        private bool IsAllowedToDelete()
        {
            if (!finishToAttack) return false;
            return gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject);
        }

        public void DeleteFromAnimationEvent()
        {
            Delete();
        }

        private void Delete()
        {
            //エフェクトを見せるためにDestroyを遅らせてる
            Destroy(sniperScope.gameObject, 4);
            Destroy(gameObject);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
