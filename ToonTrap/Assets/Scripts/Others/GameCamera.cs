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
        }

        public bool IsOutSideOfCamera(GameObject target, bool debug = false)
        {
            Vector2 screenPos = camera.WorldToViewportPoint(target.transform.position);

            if (debug) Debug.Log(screenPos);

            if (screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f) return true;
            return false;
        }

        public void Impulse()
        {
            impulseSource.GenerateImpulse();
        }
    }
}
