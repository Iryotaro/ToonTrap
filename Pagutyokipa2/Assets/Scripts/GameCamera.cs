using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken.JankenableObjects;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Ryocatusn
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        private JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

        [SerializeField]
        private Volume volume;
        [SerializeField]
        private GameManager gameManager;
        [SerializeField]
        private Player player;

        [NonSerialized]
        public new Camera camera;
        [NonSerialized]
        public LensDistortion lensDistortion;
        [NonSerialized]
        public ColorAdjustments colorAdjustments;

        private void Start()
        {
            camera = GetComponent<Camera>();

            VolumeProfile volumeProfile = volume.profile;

            if (volumeProfile.TryGet(out LensDistortion lensDistortion))
            {
                this.lensDistortion = lensDistortion;
            }
            if (volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                this.colorAdjustments = colorAdjustments;
            }
        }
    }
}