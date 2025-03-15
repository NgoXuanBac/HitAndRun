using HitAndRun.Enemy.State;
using HitAndRun.FSM;

namespace HitAndRun.Enemy
{
    public sealed class MbMonster : MbEnemy
    {
        private void Awake()
        {
            Health = 100;
            _stateMachine = new StateMachine();
            var attackState = new AttackState(this, _animator);
            var walkState = new WalkState(this, _animator);
            var idleState = new IdleState(this, _animator);
            var dyingState = new DyingState(this, _animator);

            At(idleState, walkState, new FuncPredicate(() => Target != null && Hp > 0));
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