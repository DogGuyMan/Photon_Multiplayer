using System;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Photon.Pun;
using TMPro;
using WebSocketSharp;

namespace Com.MyCompany.MyGame
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            string defaultName = string.Empty;

            defaultName = PlayerDataModel.Instance.PlayerInfo.PlayerName;
            if (defaultName.IsNullOrEmpty()) return;

            _inputField.text = defaultName;

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
            PlayerDataModel.Instance.PlayerInfo.PlayerName = value;
        }
    }
}