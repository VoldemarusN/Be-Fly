using System;

namespace Infrastructure.StateMachine
{
    public interface IState : IDisposable
    {
        void Initialize();
        void DoAction();
    }

    public interface IState<in T> : IState
    {
        void SetData(T data);
    }
}