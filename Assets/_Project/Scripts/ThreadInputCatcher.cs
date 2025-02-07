using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Jobs;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    // 오키.. 리듬게임을 만들때, Update타임보다 더 짧은 틱으로 인풋을 받을 수는 없겠구나.
    public struct InputCatchJob : IJob
    {

        public int TickCnt;
        public void Execute()
        {
            while (true)
            {
                if(ThreadInputCatcher.IsFisnish == true)
                {
                    break;
                }
                TickCnt++;
                Debug.Log($"TickCnt: {TickCnt}");
                // Input.GetKeyDown(KeyCode.Space) is not working in Job
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Pressed");
                }
            }
        }
    }

    public class ThreadInputCatcher : MonoBehaviour
    {
        private static bool isFisnish;
        public static bool IsFisnish => isFisnish;

        private void Awake()
        {
        }

        private JobHandle handle;

        private void OnEnable()
        {
            // isFisnish = false;
            // InputCatchJob inputCatchJob = new InputCatchJob();
            // inputCatchJob.TickCnt = 0;
            //
            // handle = inputCatchJob.Schedule();
            //
            // Debug.Log("InputLooper Started");
        }

        private void OnDisable()
        {
            // if (isFisnish != true)
            // {
            //     isFisnish = true;
            //     handle.Complete();
            //     Debug.Log("InputLooper Quit");
            // }
        }

        private void OnDestroy()
        {
            // OnDisable();
            // Debug.Log("ThreadInputCatcher Destroy");
        }

    }
}