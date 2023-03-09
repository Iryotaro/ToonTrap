using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using Ryocatusn.Util;

namespace Ryocatusn
{
    public class VirtualCameraManager
    {
        public static VirtualCameraManager instance = new VirtualCameraManager();
        private VirtualCameraManager() { brain = Camera.main != null ? Camera.main.GetComponent<CinemachineBrain>() : null; }

        public CinemachineBrain brain;
        private List<VirtualCamera> virtualCameras = new List<VirtualCamera>();

        public void Save(VirtualCamera virtualCamera)
        {
            virtualCameras.Add(virtualCamera);
        }
        public Option<VirtualCamera> FindEnableCamera()
        {
            VirtualCamera[] virtualCameras = this.virtualCameras.OrderBy(x => x.GetPriority()).ToArray();
            if (virtualCameras.Length == 0) return new Option<VirtualCamera>(null);
            return new Option<VirtualCamera>(virtualCameras[virtualCameras.Length - 1]);
        }
        public void Delete(VirtualCamera virtualCamera)
        {
            virtualCameras.Remove(virtualCamera);
        }
    }
}
