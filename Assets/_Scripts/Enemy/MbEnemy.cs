using System;
using HitAndRun.Bullet;
using HitAndRun.Character;
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
        public Action OnDead;
        public Vector3? Target { get; set; }
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
            _collider = GetComponent<Collider>();
            _damage = transform.Find("Damage");
            _hpBar = transform.Find("Canvas").GetComponentInChildren<Slider>();

            _collider.enabled = true;
            _attackBox.enabled = true;
            Target = null;
        }


        protected virtual void Update()
        {
            _stateMachine?.Update();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine?.FixedUpdate();
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
            if (!other.TryGetComponent(out MbCharacter character)) return;
            Target = null;
            Debug.Log($"Detect {character.name}");
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MbBullet bullet) || Target == null) return;
            MbFloatingTextSpawner.Instance.Spawn(_damage.position, _damage, bullet.Damage.ToString());
            TakeDamage(bullet.Damage);
        }

    }
}
