namespace Infrastructure.StateMachine.States
{
    public interface IState:IExitebleState
    {
        void Enter();
    }

    public interface IPayloadState<TPayload>:IExitebleState
    {
        void Enter(TPayload payload);
        
    }

    public interface IExitebleState
    {
        void Exit();
    }
}