using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    // 사용자 인터페이스에서 필요한
    // 최소로 필요한 기능은 룸에서 나갈 수 있도록 하는 것.
    // 이를 위해 UI Button 필요, 또한, Photon 메서드를 호출하는 스크립트도 필요
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Static Field

        public static GameManager _instance;
        public static GameManager Instance => _instance;

        #endregion

        #region Serialzed Field

        // 룸에 들어갔을 때 바로 인스턴스를 생성할 필요가 있으며,
        // 우리가 경기장을 로드 했다는 것을 의미 하는 GameManager 스크립트 Start() 에서 할 수 있습니다.
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        DefaultPool punPrefabPool = PhotonNetwork.PrefabPool as DefaultPool;

        #endregion

        #region Monobehaviour Callbacks

        private void Start()
        {
            _instance = this;
            if (punPrefabPool.ResourceCache.TryAdd(playerPrefab.name, playerPrefab))
            {
                Debug.LogError("이미 존재하는 키입니다.");
            }

            if(playerPrefab == null) {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                // 지금 룸에 들어와 있다면 캐릭터를 소환하기.
                // PhotonNetwork.Instantiate 명령으로 인스턴시에이트 하면 Photon이 이 프리펩을 Sync 해줄 것이다.
                if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }

            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnJoinedRoom()
        {
            // 이 함수는 MonovBehaviour가 생성되거나, 활성화 되지 않았을때 호출되지 않습니다.
            // 따라서 이 함수는 Start() 함수 또한 로컬플레이어 캐릭터가 Network.Instantiate 되었을 때를 체크한다.
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        public override void OnLeftRoom()
        {
            // Non-Master Clients
            SceneManager.LoadScene("Launcher");
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("GameManager: OnPlayerEnteredRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("GameManager: OnPlayerLeftRoom() {0}", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        #endregion

        #region Private Methods

        private void LoadArena()
        {
            // PhotonNetwork.LoadLevel() 은 마스터 클라이언트인 경우에만 호출 되어야 합니다.
            // 따라서 PhotonNetwork.isMasterClient 를 이용하여 마스터 클라이어인트인지를 체크 합니다.
            if (!PhotonNetwork.IsMasterClient)
            {
                // Non-Master Clients
                Debug.LogError("PhotonNetwork : 오직 마스터 클라이언트만 맵의 로드 책임이 있습니다. Non-Master Clients는 스킵!");
                return;
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            // 이렇게 한다면 마스터가 메서드를 호출하고 나머지 클라이언트들에게 RPC를 보내게 됩니다.
            // 어떻게 이렇게 동작하느냐?
                /* Launcher.cs*/
                // PhotonNetwork.automaticallySyncScene 을 사용하도록 해놓았기 때문에
                // 룸 안의 모든 접속한 클라이언트에 대해 이 레벨 로드를 유니티가 직접 하는 것이 아닌 Photon 이 하도록 하였습니다.
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            // Non-Master Clients
            PhotonNetwork.LeaveRoom(); // LeaveRoom() 메서드를 호출하면 "override void OnLeftRoom()" 콜백이 호출됩니다.
        }

        #endregion

    }
}