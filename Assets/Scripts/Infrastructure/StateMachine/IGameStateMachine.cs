using Infrastructure.States;

namespace Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IPayloadState<TPayLoad>;
    }
}