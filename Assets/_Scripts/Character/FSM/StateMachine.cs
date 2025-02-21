using System;
using System.Collections.Generic;

namespace HitAndRun.Character.FSM
{
    public class StateMachine
    {
        private StateNone _current;
        private Dictionary<Type, StateNone> _nodes = new();
        private HashSet<ITransition> _anyTransitions = new();
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            _current.State?.Update();
        }
        public void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }
        public void SetState(IState state)
        {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter();
        }
        public void AddTransition(IState from, IState to, IPredicate condition)
            => GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);

        public void AddAnyTransition(IState to, IPredicate condition)
            => _anyTransitions.Add(new Transition(to, condition));
            
        private StateNone GetOrAddNode(IState state)
        {
            var node = _nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNone(state);
                _nodes.Add(state.GetType(), node);
            }
            return node;
        }
        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Evealuate())
                    return transition;

            foreach (var transition in _current.Transitions)
                if (transition.Condition.Evealuate())
                    return transition;
            return null;
        }
        private void ChangeState(IState state)
        {
            if (state == _current.State) return;
            var previous = _current.State;
            var next = _nodes[state.GetType()].State;

            previous?.OnExit();
            next?.OnEnter();
            _current = _nodes[state.GetType()];
        }
        private class StateNone
        {
            public IState State { get; set; }
            public HashSet<ITransition> Transitions { get; }
            public StateNone(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }
            public void AddTransition(IState to, IPredicate condition)
                => Transitions.Add(new Transition(to, condition));
        }
    }

}
