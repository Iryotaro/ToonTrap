using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ryocatusn
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private Volume volume;

        [NonSerialized]
        public new Camera camera;
        private CinemachineImpulseSource impulseSource;

        private void Start()
        {
            camera = GetComponent<Camera>();
            impulseSource = GetComponent<CinemachineImpulseSource>();

            VolumeProfile volumeProfile = volume.profile;
        }

        public bool IsOutSideOfCamera(GameObject target)
        {
            Vector2 screenPos = (Vector2)camera.WorldToViewportPoint(target.transform.position);

            if (screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f) return true;
            return false;
        }

        public void Impulse()
        {
            impulseSource.GenerateImpulse();
        }
    }
}
