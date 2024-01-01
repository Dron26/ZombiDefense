namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private LoadingCurtain _loadingCurtain;
        
        public GameLoopState(GameStateMachine stateMachine, LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.StartLoading();
        }

        public void Exit()
        {
        }
    }
}