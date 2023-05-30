using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;

        [Inject]
        private StageManager stageManager;

        [SerializeField]
        private bool focusPlayer;
        [SerializeField]
        private bool firstCamera;

        private void Start()
        {
            VirtualCameraManager.instance.Save(this);

            virtualCamera = GetComponent<CinemachineVirtualCamera>();

            stageManager.SetupStageEvent
                .Subscribe(x => { if (focusPlayer) FocusPlayer(x.player); })
                .AddTo(this);

            if (firstCamera) SetEnableCamera();
        }
        private void OnDestroy()
        {
            VirtualCameraManager.instance.Delete(this);
        }

        private void FocusPlayer(Player player)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }

        public int GetPriority()
        {
            return virtualCamera.Priority;
        }

        public void SetEnableCamera()
        {
            //CameraBlendsのため
            if (firstCamera) name = "FirstVirtualCamera";

            VirtualCameraManager.instance.FindEnableCamera().Match(x => x.SetDisableCamera());

            virtualCamera.Priority = 10;
        }
        private void SetDisableCamera()
        {
            //Blendをデフォルトに戻す
            if (firstCamera)
            {
                name = "VirtualCamera";
                firstCamera = false;
            }

            virtualCamera.Priority = 0;
        }
    }
}
