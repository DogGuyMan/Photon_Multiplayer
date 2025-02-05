using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Com.MyCompany.MyGame.Title
{
    public class LoadLocalDataTitleTask : ITitleTask
    {
        private bool isComplete = false;
        private ProgressCallback progressCallback;

        public ITitleTaskPriority Priority => ITitleTaskPriority.High;

        public void Initialize(ProgressCallback getProgress)
        {
            this.progressCallback = getProgress;
            progressCallback.Invoke(GetHashCode(), 0f);
        }

        public async UniTask RunTask()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "playerinfo.json");
            if (!File.Exists(filePath))
            {
                await PlayerDataModel.Instance.SavePlayerData(filePath);
                throw new FileNotFoundException("파일이 존재하지 않습니다.");
            }

            await PlayerDataModel.Instance.LoadPlayerData(filePath);

            isComplete = true;
            progressCallback.Invoke(GetHashCode(), 1f);
        }

        public (bool, string) HasError()
        {
            if (!isComplete)
            {
                return (true, "아직 처리중");
            }

            return (false, null);
        }

        public UniTask HandleError()
        {
            throw new System.NotImplementedException();
        }
    }
}