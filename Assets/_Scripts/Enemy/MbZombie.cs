using HitAndRun.Character;
using HitAndRun.Enemy.State;
using HitAndRun.FSM;

namespace HitAndRun.Enemy
{
    public sealed class MbZombie : MbEnemy
    {
        private void Awake()
        {
            _stateMachine = new StateMachine();
            var attackState = new AttackState(this, _animator);
            var walkState = new WalkState(this, _animator);
            var idleState = new IdleState(this, _animator);
            var dyingState = new DyingState(this, _animator);

            At(idleState, walkState, new FuncPredicate(() => _autoTarget.Target != null && Hp > 0));
            Any(idleState, new FuncPredicate(() => _autoTarget.Target == null && Hp > 0));
            Any(dyingState, new FuncPredicate(() => Hp <= 0));

            _stateMachine?.SetState(typeof(IdleState));
        }

        public override void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        protected override void Attack(MbCharacter character)
        {
            throw new System.NotImplementedException();
        }
    }

}