using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using DG.Tweening;
using Ryocatusn.Util;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn
{
    public class HandQueue : MonoBehaviour, IHandForSpriteEditor
    {
        private JankenableObjectApplicationService jankenableObjectApplicationService;
        private AttackableObjectApplicationService attackableObjectApplicationService;
        private JankenableObjectId id;

        private IReceiveAttack receiveAttack;

        [SerializeField]
        private Hand.Shape shape;
        
        [SerializeField]
        private HandSprites handSprites;

        private Option<Tween> rotateTween = new Option<Tween>(null);

        private void Start()
        {
            jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();
            attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, StageManager.activeStage.id);
            id = jankenableObjectApplicationService.Create(command);

            JankenableObjectEvents events = jankenableObjectApplicationService.GetEvents(id);

            events.AttackTriggerEvent
                .Subscribe(x => Attack(x.id))
                .AddTo(this);

            Collider2D[] childrenColliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D childrenCollider in childrenColliders)
            {
                childrenCollider.gameObject.OnTriggerEnter2DAsObservable()
                    .Subscribe(x => OnHit(x));
            }

            SetRotate();
        }
        private void OnDestroy()
        {
            jankenableObjectApplicationService.Delete(id);
        }

        private void OnHit(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IReceiveAttack receiveAttack))
            {
                this.receiveAttack = receiveAttack;
                jankenableObjectApplicationService.AttackTrigger(id, new AttackableObjectCreateCommand(id, shape, new Atk(1)));
            }
        }

        private void Attack(AttackableObjectId attakableObjectId)
        {
            attackableObjectApplicationService.Attack(attakableObjectId, receiveAttack);
        }

        public SpriteRenderer[] GetSpriteRenderers()
        {
            return GetComponentsInChildren<SpriteRenderer>();
        }
        public Hand.Shape GetHandShape()
        {
            return shape;
        }
        public HandSprites GetHandSprites()
        {
            return handSprites;
        }

        private void SetRotate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.transform.eulerAngles = Vector3.zero;
                rotateTween.Set(child.transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(-1).SetLink(gameObject));
            }
        }
    }
}
