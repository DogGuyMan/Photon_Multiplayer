using Cysharp.Threading.Tasks;
using Photon.Pun;
using Pun2Task;

namespace Com.MyCompany.MyGame.Title
{
    public class ConnectPhotonServerTitleTask : ITitleTask
    {
        private bool isComplete = false;
        public ITitleTaskPriority Priority => ITitleTaskPriority.High;
        private ProgressCallback progressCallback;
        public void Initialize(ProgressCallback getProgress)
        {
            this.progressCallback = getProgress;
            progressCallback.Invoke(GetHashCode(), 0f);
        }

        public async UniTask RunTask()
        {
            PhotonNetwork.GameVersion = "1.0.0";
            // "PhotonServerSetting.Asset"에 저장된 세팅을 가지고
            // 이때가 바로 Photon Cloud에 연결되는 시작 지점이다.
            await Pun2TaskNetwork.ConnectUsingSettingsAsync();

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