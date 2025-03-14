using HitAndRun.Enemy.State;
using HitAndRun.FSM;

namespace HitAndRun.Enemy
{
    public sealed class MbZombie : MbEnemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();
            var attackState = new AttackState(_animator);
            var walkState = new WalkState(_animator);
            var idleState = new IdleState(_animator);
            var dyingState = new DyingState(_animator);

            Any(idleState, new FuncPredicate(() => Target == null && Hp > 0));
            Any(dyingState, new FuncPredicate(() => Hp <= 0));

            _stateMachine?.SetState(typeof(IdleState));
        }

        public override void TakeDamage(int damage)
        {
            Hp -= damage;
        }
    }

}