using Service;

namespace Services.PauseService
{
    public interface IPauseService:IService
    {
        void SetPause(bool isPaused);
        void Resume();
        bool IsPaused { get; }
    }
}