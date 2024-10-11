namespace Services.PauseService
{
    public interface IPauseListener
    {
        void OnPaused();
        void OnResumed();
    }
}