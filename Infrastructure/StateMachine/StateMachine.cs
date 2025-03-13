namespace Infrastructure.StateMachine
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void SetState(IState state)
        {
            CurrentState?.Dispose();
            CurrentState = state;
            CurrentState.Initialize();
        }
    }
}