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
        [SerializeField] private Rigidbody _rb;
        [SerializeField] Slider _hpBar;
        [SerializeField] protected MbAutoTarget _autoTarget;
        [SerializeField, Range(0, 10)] private float _moveSpeed = 2f;
        public Action OnDead;
        public bool IsAttacking { get; set; }
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
                    _hpBar.gameObject.SetActive(false);
                    OnDead?.Invoke();
                }
            }
        }
        protected StateMachine _stateMachine;
        protected void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        protected void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        public virtual void Reset()
        {
            _animator = GetComponentInChildren<Animator>();
            _autoTarget ??= GetComponentInChildren<MbAutoTarget>();
            _collider = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
            _damage = transform.Find("Damage");
            _hpBar ??= transform.Find("Canvas").GetComponentInChildren<Slider>();

            _hpBar.gameObject.SetActive(true);
            _collider.enabled = true;
            _autoTarget.enabled = true;

            IsAttacking = false;
        }

        protected virtual void OnEnable()
        {
            _stateMachine?.SetState(typeof(IdleState));
        }

        protected virtual void Awake()
        {
            Health = 100;
            _stateMachine = new StateMachine();
            var attackState = new AttackState(this, _animator);
            var walkState = new WalkState(this, _animator);
            var idleState = new IdleState(this, _animator);
            var dyingState = new DyingState(this, _animator);

            At(idleState, walkState, new FuncPredicate(() => _autoTarget.Target && Hp > 0));
            Any(idleState, new FuncPredicate(() => !_autoTarget.Target && !IsAttacking && Hp > 0));
            At(attackState, walkState, new FuncPredicate(() => Hp > 0 && !IsAttacking));
            Any(attackState, new FuncPredicate(() => Hp > 0 && IsAttacking));

            Any(dyingState, new FuncPredicate(() => Hp <= 0));

            _stateMachine?.SetState(typeof(IdleState));
        }

        protected virtual void Update()
        {
            _stateMachine?.Update();
            DetectCharacter();
        }

        private void DetectCharacter()
        {
            if (_stateMachine.GetCurrentState() is not WalkState) return;
            var origin = transform.position + Vector3.up * _collider.bounds.extents.y;
            var direction = (_autoTarget.Target.position - transform.position).normalized;
            var maxDistance = _collider.bounds.extents.z * 3;

            if (Physics.Raycast(origin, direction, out var hit, maxDistance)
                && hit.collider.TryGetComponent(out MbCharacter character))
            {
                IsAttacking = true;
                Attack(character);
            }
#if UNITY_EDITOR
            Debug.DrawRay(origin, direction * maxDistance, Color.red);
#endif
        }
        protected virtual void FixedUpdate()
        {
            _stateMachine?.FixedUpdate();
            MoveTowardsTarget();
        }


        private void MoveTowardsTarget()
        {
            _rb.velocity = Vector3.zero;
            if (_stateMachine.GetCurrentState() is not WalkState) return;
            else if (_autoTarget.Target == null) return;

            var direction = (_autoTarget.Target.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * _moveSpeed * Time.fixedDeltaTime);

            if (direction.magnitude > 0.1f)
                transform.rotation = Quaternion.LookRotation(direction);
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
