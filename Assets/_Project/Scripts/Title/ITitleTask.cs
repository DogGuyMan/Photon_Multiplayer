using Cysharp.Threading.Tasks;

namespace Com.MyCompany.MyGame.Title
{
    public delegate void ProgressCallback(int hashCode, float progress);
    public enum ITitleTaskPriority
    {
        High,
        Middle,
        Low
    }

    public interface ITitleTask
    {
        public ITitleTaskPriority Priority { get; }

        void Initialize(ProgressCallback getProgress);
        UniTask RunTask();
        (bool, string) HasError();
        UniTask HandleError();
    }
}