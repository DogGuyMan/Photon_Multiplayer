using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        private Animator animator;
        [SerializeField]
        //  왼쪽 또는 오른쪽 키를 누를 때 캐릭터가 회전을 갑자기 하는 것 보다는 완만하고 부드럽게 회전 하는 것이 더 좋습니다.
        private float directionDampTime = 0.25f;


        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            animator ??= GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            // 인스턴스가 'client' 어플리케이션에서 제어되고 있다면 photonView.IsMine은 true일 것 입니다.
            if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if(!animator) return;
            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            //게임에서는 뒤로 가는 것을 허용하지 않기 때문에 v 가 0 보다 작은 값인지 확인 하세요.
            //사용자가 '아래 화살표'' 키 또는 's' 키 (Vertical 축에 대한 기본 설정) 를 입력하면
            //허용하지 않고 값을 0 으로 설정합니다.
            // 두 입력값을 제곱하고 있다는 것을 알아 챘을 것 입니다. 왜 그럴까요?
            // 항상 양의 절대 값이고 easing을 추가하기 때문 입니다. 절묘한 트릭입니다.
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}