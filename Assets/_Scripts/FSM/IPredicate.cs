using System;

namespace HitAndRun.FSM
{
    public interface IPredicate
    {
        bool Evealuate();
    }

    public class FuncPredicate : IPredicate
    {
        private Func<bool> _func;
        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }
        public bool Evealuate()
        {
            return _func.Invoke();
        }
    }
}

