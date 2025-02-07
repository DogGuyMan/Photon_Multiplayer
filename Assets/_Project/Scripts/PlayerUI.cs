using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Realtime;
using TMPro;

namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);
        float characterControllerHeight = 0f;
        Transform TargetTransform => target.transform;
        Vector3 TargetPosition => target.transform.position;
        PlayerManager target;

        private bool isInitialized = false;
        public bool IsInitialized => isInitialized;

        #endregion

        #region Public Fields

        public PlayerUI Initialize()
        {
            isInitialized = true;
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            return this;
        }

        // 흠.. 이런걸 바인딩이라고 하는구나.
        public void SetTarget(PlayerManager targetRef)
        {
            if(isInitialized == false)
            {
                Debug.LogError("PlayerUI is not initialized", this);
                return;
            }

            if(targetRef == null)    {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }

            target = targetRef;
            CharacterController characterController = target.GetComponent<CharacterController>();
            if(characterController != null)
            {
                characterControllerHeight = characterController.height;
            }

            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }

            targetRef.OnPlayerDeathEvent += PlayerDeathHandler;
        }

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }
        }

        private void LateUpdate()
        {
            if (TargetTransform != null)
            {
                // 월드의 좌표가 스크린에 표시되는 좌표로 변환됩니다.
                this.transform.position = Camera.main.WorldToScreenPoint(TargetPosition) + screenOffset + characterControllerHeight * Vector3.up;
            }
        }

        #endregion

        #region Public Methods

        private void PlayerDeathHandler([CanBeNull] object sender, EventArgs e)
        {
            if (sender is PlayerManager)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        #endregion

    }
}