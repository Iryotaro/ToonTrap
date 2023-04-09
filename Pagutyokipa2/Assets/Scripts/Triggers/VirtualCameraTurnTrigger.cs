using Ryocatusn.TileTransforms;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class VirtualCameraTurnTrigger : MonoBehaviour
    {
        [SerializeField]
        private VirtualCameraAndDirection[] virtualCameras;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnTriggerExitPlayerEvent
                .Subscribe(x => ChangeCamera(x))
                .AddTo(this);
        }
        private void ChangeCamera(TileDirection.Direction direction)
        {
            VirtualCameraAndDirection targetCamera = virtualCameras.Where(x => x.direction.Equals(direction)).FirstOrDefault();
            if (targetCamera == null) return;
            targetCamera.virtualCamera.SetEnableCamera();
        }

        [Serializable]
        private class VirtualCameraAndDirection
        {
            public VirtualCamera virtualCamera;
            public TileDirection.Direction direction;
        }
    }
}
