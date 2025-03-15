using System;
using HitAndRun.Bullet;
using HitAndRun.Character;
using HitAndRun.Enemy.State;
using HitAndRun.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Enemy
{
    public abstract class MbEnemy : MbDamageable
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] private Transform _damage;
        [SerializeField] private Collider _collider;
        [SerializeField] Slider _hpBar;
        [SerializeField] private MbCollider _attackBox;
        [SerializeField] protected MbAutoTarget _autoTarget;
        [SerializeField, Range(0, 10)] private float _moveSpeed = 2f;
        public Action OnDead;
        private bool _isAttacking;
        private long _health;
        public long Health
        {
            get => _health;
            set
            {
                _health = value;
                Hp = value;
            }
        }
        private long _hp;
        public long Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                var progress = Mathf.Clamp01((float)_hp / Health);
                _hpBar.value = progress;
                if (_hp <= 0)
                {
                    _collider.enabled = false;
                    _attackBox.enabled = false;
                    _hpBar.gameObject.SetActive(false);
                    OnDead?.Invoke();
                }
            }
        }
        protected StateMachine _stateMachine;
        protected void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        protected void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        protected virtual void Reset()
        {
            _animator = GetComponentInChildren<Animator>();
            _attackBox ??= GetComponentInChildren<MbCollider>();
            _autoTarget ??= GetComponentInChildren<MbAutoTarget>();
            _collider = GetComponent<Collider>();
            _damage = transform.Find("Damage");
            _hpBar = transform.Find("Canvas").GetComponentInChildren<Slider>();

            _hpBar.gameObject.SetActive(true);
            _collider.enabled = true;
            _attackBox.enabled = true;
            _autoTarget.enabled = true;
        }

        protected virtual void Awake()
        {
            Health = 100;
            _stateMachine = new StateMachine();
            var attackState = new AttackState(this, _animator);
            var walkState = new WalkState(this, _animator);
            var idleState = new IdleState(this, _animator);
            var dyingState = new DyingState(this, _animator);

            At(idleState, walkState, new FuncPredicate(() => _autoTarget.Target != null && Hp > 0));
            Any(idleState, new FuncPredicate(() => _autoTarget.Target == null && Hp > 0));
            Any(dyingState, new FuncPredicate(() => Hp <= 0));
            Any(attackState, new FuncPredicate(() => Hp > 0 && !_isAttacking));

            _stateMachine?.SetState(typeof(IdleState));
        }

        protected virtual void Update()
        {
            _stateMachine?.Update();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine?.FixedUpdate();
            MoveTowardsTarget();
        }


        private void MoveTowardsTarget()
        {
            var canMove = _isAttacking && _autoTarget.Target != null;
            if (canMove)
            {
                var direction = (_autoTarget.Target.position - transform.position).normalized;

                transform.position += direction * _moveSpeed * Time.fixedDeltaTime;

                if (direction.magnitude > 0.1f)
                    transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        private void OnEnable()
        {
            _attackBox.TriggerEnter += OnDetect;
        }

        private void OnDisable()
        {
            _attackBox.TriggerEnter -= OnDetect;
        }

        private void OnDetect(GameObject other)
        {
            _isAttacking = false;
            if (!other.TryGetComponent(out MbCharacter character)) return;
            _isAttacking = true;
            Attack(character);
        }

        protected abstract void Attack(MbCharacter character);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MbBullet bullet) || _autoTarget.Target == null) return;
            MbFloatingTextSpawner.Instance.Spawn(_damage.position, _damage, bullet.Damage.ToString());
            TakeDamage(bullet.Damage);
        }

    }
}
