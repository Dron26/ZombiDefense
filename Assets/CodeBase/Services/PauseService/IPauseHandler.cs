namespace Services.PauseService
{
    public interface  IPauseHandler:IService
    {
        void SetPaused(bool isPaused);
    }
}