using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HitAndRun.Bullet;
using HitAndRun.Character.FSM;
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

        private StateMachine _stateMachine = new();
        private CancellationTokenSource _cts = new();
        public const string ACTIVE_TAG = "Character";
        public const string INACTIVE_TAG = "Character_Inactive";

        public Action<MbCharacter> OnDead;
        public bool IsMerging { get; set; }
        private bool _isHit;

        public void Reset()
        {
            _body ??= GetComponent<MbCharacterBody>();
            _shooter = transform.Find("Shooter");
            _grabber = GetComponentInChildren<MbGrabber>();
            _animator = GetComponentInChildren<Animator>();
            transform.position = Vector3.zero;
            _body.Reset();

            _fireRate = MbGameManager.Instance.Specifications.FireRate;
            _damage = MbGameManager.Instance.Specifications.Damage;

            _isHit = false;
            Left = Right = null;
            IsMerging = false;
            tag = INACTIVE_TAG;
        }

        private void Awake()
        {
            var idleState = new IdleState(this, _animator);
            var runState = new RunState(this, _animator);
            var dyingState = new DyingState(this, _animator);
            var fallState = new FallState(this, _animator);

            At(idleState, runState, new FuncPredicate(() => tag == ACTIVE_TAG));
            Any(fallState, new FuncPredicate(() => tag == ACTIVE_TAG && !_body.IsGrounded));
            Any(dyingState, new FuncPredicate(() => tag == ACTIVE_TAG && _isHit));
            Any(idleState, new FuncPredicate(() => tag == INACTIVE_TAG));

            OnDead += character => character.Body.StopTween();
            SetDefaultState();
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        public void SetDefaultState()
        {
            _stateMachine.SetState(typeof(IdleState));
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            ProcessShootingAsync().Forget();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private async UniTaskVoid ProcessShootingAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => this != null && _stateMachine.GetCurrentState() is RunState);
                _shootingPattern.Shoot(_bulletSpeed, _shooter, _body.Color, transform.localScale, _damage * _body.Level);
                await UniTask.Delay((int)(_fireRate * 1000));
            }
        }

        private void OnDisable()
        {
            _cts.Cancel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tower") _isHit = true;
            else if (other.TryGetComponent(out MbModifierBase modifier))
            {
                modifier.Modify(this);
            }

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

