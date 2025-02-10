using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
#region Private Serialized

        /// <summary>
        /// 룸에 참여할 수 있는 최대 플레이어 입니다.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;


#endregion

#region Private Fields

        /// <summary>
        /// 클라이언트 버젼 넘버, 유저는 이 클라이언트 버젼에 따라 분리된다.
        /// </summary>
        private string gameVersion = "1";
        private bool isConnecting;

        // 이 값은 당연히 씬 전환 되고 Start()이 호출 될떄마다. 값이 초기화 된다.
        // 따라서 이 값은 씬 전환을 통해 이전 씬의 값이 유지되지 않는다.
        private bool IsConnecting
        {
            get
            {
                Debug.Log($"PUN Basics Tutorial/Launcher: IsConnecting Getted {isConnecting}");
                return isConnecting;
            }
            set
            {
                Debug.Log($"PUN Basics Tutorial/Launcher: IsConnecting Setted {value}");
                isConnecting = value;
            }
        }


        #endregion

#region MonoBehaviour Callbacks

        private void Awake()
        {
            // Critical
            // 이 값이 참일때, MasterClient는 PhotonNetwork.LoadLevel()을 호출할 수 있게 되고.
            // 모든 연결된 플레이어들은 동일한 레벨을 자동적으로 로드 할 것입니다.

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            IsConnecting = false;
        }

#endregion

#region Public Methods

        /// <summary>
        /// 연결 시도를 시작한다
        /// - 만약 이미 연결되었다면, 랜덤한 방으로 접근 시도할 것
        /// - 만약 연결이 안되었으면 포톤 클라우드 네트워크에 연결 시도할 것
        /// </summary>
        public void Connect()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: Connect() was called by PUN");
            IsConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings(); // 이때가 바로 Photon Cloud에 연결되는 시작 지점이다.
            }
        }

#endregion

#region MonoBehaviourPunCallbacks Callbacks



        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if(IsConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomRoomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // 방을 생성하고, 방의 설정을 정의한다.
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

#endregion
    }
}
