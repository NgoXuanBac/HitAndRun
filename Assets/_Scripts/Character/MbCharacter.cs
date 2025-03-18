using System;
using HitAndRun.Bullet;
using HitAndRun.Character.State;
using HitAndRun.FSM;
using UnityEngine;

namespace HitAndRun.Character
{
    [RequireComponent(typeof(MbCharacterBody))]
    public class MbCharacter : MonoBehaviour
    {
        public MbCharacter Left { get; set; }
        public MbCharacter Right { get; set; }
        [SerializeField] private MbCharacterBody _body;
        public MbCharacterBody Body => _body;

        public IShootingPattern ShootingPattern { get; set; } = new SingleShot();

        [SerializeField] private Transform _shooter;
        [SerializeField] private Animator _animator;
        public int FireRate { get; set; } = 2;
        public int Damage { get; set; } = 2;
        [SerializeField, Range(1, 100)] private float _bulletSpeed = 50f;
        [SerializeField] MbAutoTarget _autoTarget;
        private StateMachine _stateMachine;
        public Action<MbCharacter> OnDead;
        public event Action<MbCharacter, MbCharacter, bool> OnGrab;
        public bool IsMerging { get; set; }
        public bool IsAttack { get; set; }
        private bool _isDead;
        private float _nextFireTime;

        public bool IsActive
        {
            get => tag == "Character";
            set => tag = value ? "Character" : "Character_Inactive";
        }
        public void Reset()
        {
            _body ??= GetComponent<MbCharacterBody>();
            _autoTarget ??= GetComponentInChildren<MbAutoTarget>();
            _shooter = transform.Find("Shooter");
            _animator = GetComponentInChildren<Animator>();
            transform.position = Vector3.zero;
            _body.Reset();

            _isDead = false;
            IsActive = false;
            Left = Right = null;

            IsMerging = IsAttack = false;
            _stateMachine?.SetState(typeof(IdleState));
        }

        private void OnEnable()
        {
            FireRate = MbGameManager.Instance.Data.FireRate;
            Damage = MbGameManager.Instance.Data.Damage;
            ShootingPattern = new SingleShot();
        }

        private void Awake()
        {
            _stateMachine = new StateMachine();
            var idleState = new IdleState(this, _animator);
            var runState = new RunState(this, _animator);
            var dyingState = new DyingState(this, _animator);
            var fallState = new FallState(this, _animator);
            var attackState = new AttackState(this, _animator);

            At(idleState, runState, new FuncPredicate(() => IsActive));
            At(runState, attackState, new FuncPredicate(() => IsAttack));
            Any(fallState, new FuncPredicate(() => IsActive && !_body.IsGrounded));
            Any(dyingState, new FuncPredicate(() => IsActive && _isDead));
            Any(idleState, new FuncPredicate(() => !IsActive));

            OnDead += character => character.Body.Unaffected();
            _stateMachine?.SetState(typeof(IdleState));
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            _stateMachine.Update();
            _autoTarget.enabled = _stateMachine.GetCurrentState() is AttackState;
            if (_stateMachine.GetCurrentState() is RunState or AttackState)
            {
                if (Time.time <= _nextFireTime) return;

                if (_autoTarget.enabled && _autoTarget.Target != null)
                {
                    var direction = _autoTarget.Target.transform.position - _shooter.position;
                    direction.y = 0;
                    if (direction != Vector3.zero) _shooter.rotation = Quaternion.LookRotation(direction);
                }
                else _shooter.rotation = Quaternion.identity;

                _shooter.rotation = Quaternion.Euler(0, _shooter.rotation.eulerAngles.y, 0);
                ShootingPattern.Shoot(_bulletSpeed, _shooter, _body.Color, transform.localScale, Damage * _body.Level);
                _nextFireTime = Time.time + 0.1f + (0.5f - 0.1f) * Mathf.Exp(-0.2f * FireRate);
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tower" || other.tag == "Obstacle")
                _isDead = true;
            else if (!other.CompareTag("Character_Inactive")) return;
            var dir = (other.transform.position - transform.position).normalized;
            var side = Vector3.Dot(dir, transform.right);

            OnGrab?.Invoke(this, other.GetComponent<MbCharacter>(), side > 0);
        }

        public void TakeDamage()
        {
            _isDead = true;
        }
    }
}

