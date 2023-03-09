using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UnityEngine;

using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn
{
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    [RequireComponent(typeof(Sprite))]
    public class JankenLight : MonoBehaviour, IHandForSpriteEditor
    {
        private HandApplicationService handApplicationService;
        private HandId id;

        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private HandSprites handSprites;

        private void Start()
        {
            handApplicationService = Installer.installer.serviceProvider.GetService<HandApplicationService>();

            id = handApplicationService.Create(shape);

            UnityEngine.Rendering.Universal.Light2D light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x =>
                {
                    JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

                    JankenableObjectEvents events = jankenableObjectApplicationService.GetEvents(x.player.id);

                    if (jankenableObjectApplicationService.Get(x.player.id).shape == handApplicationService.Get(id).shape) TurnOn(light);
                    else TurnOff(light);

                    events.ChangeShapeEvent
                    .Subscribe(x => 
                    {
                        if (x == handApplicationService.Get(id).shape) TurnOn(light);
                        else TurnOff(light);
                    })
                    .AddTo(this);
                });
        }
        private void OnDestroy()
        {
            handApplicationService.Delete(id);
        }

        private void TurnOn(UnityEngine.Rendering.Universal.Light2D light)
        {
            StageManager.activeStage.gameContains.Match(gameContains =>
            {
                if (gameContains.globalLight.intensity == 0) light.intensity = 1;
                else light.intensity = 0.6f;
            });

            light.enabled = true;
        }
        private void TurnOff(UnityEngine.Rendering.Universal.Light2D light)
        {
            light.enabled = false;
        }

        public SpriteRenderer[] GetSpriteRenderers()
        {
            return new SpriteRenderer[] { GetComponent<SpriteRenderer>() };
        }
        public Hand.Shape GetHandShape()
        {
            return shape;
        }
        public HandSprites GetHandSprites()
        {
            return handSprites;
        }
    }
}
