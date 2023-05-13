using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System.Linq;
using System;

namespace Ryocatusn
{
    public class NetworkBehaviour : MonoBehaviour
    {
        protected PhotonView photonView;

        private void Awake()
        {
            if (TryGetComponent(out PhotonView photonView))
            {
                this.photonView = photonView;
            }
        }
        
        protected void CallRpc(string methodName, params object[] parameters)
        {
            if (photonView != null)
            {
                photonView.RPC(methodName, RpcTarget.All, parameters);
            }
            else
            {
                Type[] parameterTypes = parameters.Select(x => x.GetType()).ToArray();
                MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
                method.Invoke(this, parameters);
            }
        }

        protected void DestroyThis()
        {
            if (photonView != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
