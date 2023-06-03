using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;
using Ryocatusn.Games;

namespace Ryocatusn
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;

        [Inject]
        private GameManager gameManager;

        [SerializeField]
        private bool focusPlayer;
        [SerializeField]
        private bool firstCamera;

        private void Start()
        {
            VirtualCameraManager.instance.Save(this);

            virtualCamera = GetComponent<CinemachineVirtualCamera>();

            if (focusPlayer) FocusPlayer(gameManager.gameContains.player);
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
