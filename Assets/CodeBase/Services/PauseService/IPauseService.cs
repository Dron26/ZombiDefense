using Service;

namespace Services.PauseService
{
    public interface IPauseService:IService
    {
        void Pause();
        void Resume();
        bool IsPaused { get; }
    }
}