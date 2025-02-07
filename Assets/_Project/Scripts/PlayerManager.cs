using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    // PhotonView로 노출 시키기 위해 MonoBehaviourPunCallbacks 상속
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {

#region Serializable Fields

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

#endregion

#region Public Fields

        /// <summary>
        /// 로컬 플레이어의 인스턴스를 저장해야 한다, "프리펩이 아님!"
        /// 그래서 이것을 사용해 현재 룸에 타 플레이어가 있는지 체크한다.
        /// </summary>
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        private static GameObject localPlayerInstance;
        public static GameObject LocalPlayerInstance => localPlayerInstance;

#endregion

#region Private Fields

        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        //True, when the user is firing
        bool IsFiring;
#endregion

#region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            //  photonView.IsMine이 true 이면 이 인스턴스를 따라가야 할 필요가 있다는 의미 입니다.
            // 그래서 _cameraWork.OnStartFollowing() 를 호출 하여 신의 카메라가 바로 이 인스턴스를 따라 가도록 만듭니다.
            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();
            if (cameraWork != null)
            {
                if(photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Camera Component on PlayerPrefab.", this);
            }

            if (beams != null)
            {
                beams.SetActive(false);
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }

            if (photonView.IsMine)
            {
                PlayerManager.localPlayerInstance = this.gameObject;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

            if (playerUiPrefab != null)
            {
                Instantiate(playerUiPrefab).GetComponent<PlayerUI>().SetTarget(this);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }

        private void CalledOnLevelWasLoaded(int sceneBuildIndex)
        {
            if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
            Instantiate(playerUiPrefab).GetComponent<PlayerUI>().SetTarget(this);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            // 다른 인스턴스들도 발사가 필요 합니다! 네트워크를 경유하여 발사를 동기화 해주는 메카니즘이 필요 합니다.
            // 수동으로 IsFiring boolean 값을 동기화 할 것 입니다.
            // 여기에서는 게임에 매우 특수한 것 값을 동기화 해야 하므로 이것을 커스텀 Observable을 구현해야한다.
            if (photonView.IsMine)
            {
                ProcessInputs();
                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }


        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
        public void OnTriggerEnter(Collider other)
        {
            // PhotonView 가 Mine 이 아니면 메소드 초반부에서 빠져나왔기 때문입니다.
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;
        }
        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// We're going to affect health while the beams are touching the player
        /// </summary>
        /// <param name="other">Other.</param>
        public void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (! photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            Health -= 0.1f*Time.deltaTime;
        }

        public event Action<PlayerManager, EventArgs> OnPlayerDeathEvent;

        private void OnDestroy()
        {
            OnPlayerDeathEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

#region Public Method

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        public void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

#endregion

#region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // IsMine이 true인, 우리 자신의 플레이어라면,
                // 다른 클라이언트에게 우리 데이터를 공유(동기화)할 수 있다.
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // 만약 IsMine이 false인, 다른 플레이어라면
                // 다른 클라이언트로부터 데이터를 받아서 동기화 할 수 있다.
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }

#endregion
    }
}