using System;
using HitAndRun.Bullet;
using HitAndRun.Character.State;
using HitAndRun.FSM;
using HitAndRun.Gate.Modifier;
using UnityEditor;
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

        private IShootingPattern _shootingPattern = new SingleShot();
        public IShootingPattern ShootingPattern => _shootingPattern;

        [SerializeField] private Transform _shooter;
        [SerializeField] private MbGrabber _grabber;
        public MbGrabber Grabber => _grabber;
        [SerializeField] private Animator _animator;

        [Header("Shooting")]
        [SerializeField, Range(0, 1)] private float _fireRate = 0.2f;
        public float FireRate => _fireRate;
        [SerializeField, Range(1, 100)] private int _damage = 2;
        public float Damage => _damage;
        [SerializeField, Range(1, 100)] private float _bulletSpeed = 50f;
        [SerializeField] MbAutoTarget _autoTarget;
        private StateMachine _stateMachine;
        public Action<MbCharacter> OnDead;
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
            _grabber = GetComponentInChildren<MbGrabber>();
            _animator = GetComponentInChildren<Animator>();
            transform.position = Vector3.zero;
            _body.Reset();

            _fireRate = MbGameManager.Instance.Data.FireRate;
            _damage = MbGameManager.Instance.Data.Damage;

            _isDead = false;
            IsActive = false;
            Left = Right = null;

            IsMerging = IsAttack = false;
            _stateMachine?.SetState(typeof(IdleState));
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
                _shootingPattern.Shoot(_bulletSpeed, _shooter, _body.Color, transform.localScale, _damage * _body.Level);
                _nextFireTime = Time.time + _fireRate;
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tower") _isDead = true;
            else if (other.TryGetComponent(out MbModifierBase modifier))
            {
                modifier.Modify(this);
            }
        }

        public void TakeDamage()
        {
            _isDead = true;
        }

#if UNITY_EDITOR
        public void SetShootingPattern(IShootingPattern pattern)
        {
            _shootingPattern = pattern;
        }
#endif

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MbCharacter))]
    public class ECharacterInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var character = (MbCharacter)target;
            GUI.enabled = Application.isPlaying;
            var shooting = character.ShootingPattern;
            var text = shooting is SpreadShot ? "SingleShot" : "SpreadShot";
            if (GUILayout.Button($"Shooting Pattern: {text}"))
            {
                character.SetShootingPattern(shooting is SpreadShot ? new SingleShot() : new SpreadShot());
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif
}

