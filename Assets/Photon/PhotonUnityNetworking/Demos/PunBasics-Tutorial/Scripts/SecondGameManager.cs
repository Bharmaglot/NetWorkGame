using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;


namespace Photon.Pun.Demo.PunBasics
{
    public class SecondGameManager : MonoBehaviourPunCallbacks
    {

        static public SecondGameManager Instance;

        private GameObject instance;
        [SerializeField] private GameObject playerPrefab;



        private void Start()
        {
            Instance = this;

            //if (!PhotonNetwork.InRoom)
            //{
            //    Debug.Log("PhotonNetwork Is not Connected");
            //    Debug.Log($"{PhotonNetwork.InRoom.ToString()}");

            //    return;
            //}

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public override void OnJoinedRoom()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        //public override void OnPlayerEnteredRoom(Player other)
        //{
        //    Debug.Log("OnPlayerEnteredRoom() " + other.NickName); 

        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

        //        LoadArena();
        //    }
        //}
    }
}
