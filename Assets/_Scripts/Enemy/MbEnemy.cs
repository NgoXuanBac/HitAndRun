using HitAndRun.Bullet;
using HitAndRun.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Enemy
{
    public abstract class MbEnemy : MbDamageable
    {
        [SerializeField] private Animator _animator;
        [SerializeField]
        private Transform _damage;
        [SerializeField] Slider _hpBar;
        private StateMachine _stateMachine = new();
        [SerializeField]
        private MbCollider _attackBox;
        private long _health;
        public long Health
        {
            get => _health;
            set
            {
                _health = value;
                SetHp(value);
            }
        }
        private long _hp;
        private void SetHp(long value)
        {
            _hp = value;
            var progress = Mathf.Clamp01((float)_hp / Health);
            _hpBar.value = progress;
        }

        protected virtual void Reset()
        {
            _animator = GetComponentInChildren<Animator>();
            _attackBox ??= GetComponentInChildren<MbCollider>();
            _damage = transform.Find("Damage");
            _hpBar = transform.Find("Canvas").GetComponentInChildren<Slider>();
        }

        protected virtual void Awake()
        {
            Health = 100;
            // var attackState = new AttackState(_animator);
            // var walkState = new WalkState(_animator);
            // var idleState = new IdleState(_animator);
            // var dyingState = new DyingState(_animator);

            // _stateMachine?.SetState(typeof(IdleState));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MbBullet bullet)) return;
            MbFloatingTextSpawner.Instance.Spawn(_damage.position, _damage, bullet.Damage.ToString());
            SetHp(_hp - bullet.Damage);
        }

    }
}
