using System;

namespace HitAndRun.Character.FSM
{

    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }

    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        public Transition(IState to, IPredicate condition)
            => (To, Condition) = (to, condition);
    }
}

