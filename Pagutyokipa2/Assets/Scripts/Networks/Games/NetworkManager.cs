using Photon.Pun;
using Photon.Realtime;
using Ryocatusn.Games;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(OnLineGameManager))]
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            GameObject gameObject = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

            if (gameObject.TryGetComponent(out Player player))
            {
                GetComponent<OnLineGameManager>().StartGame(player);
            }
            else
            {
                Debug.LogError("AvatorはPlayerである必要があります");
            }
        }
    }
}
