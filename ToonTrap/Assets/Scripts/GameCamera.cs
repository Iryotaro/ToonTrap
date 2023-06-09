using Ryocatusn.Janken.JankenableObjects;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private Volume volume;

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

        public bool IsOutSideOfCamera(GameObject target)
        {
            Vector2 screenPos = camera.WorldToViewportPoint(target.transform.position);
            if (screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f) return true;
            return false;
        }
    }
}
