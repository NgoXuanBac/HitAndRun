using System;
using System.Collections;
using HitAndRun.Bullet;
using TMPro;
using UnityEngine;

namespace HitAndRun.Tower
{
    public class MbTower : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private Animator _animator;
        [SerializeField] private MbCollider _collider;
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
                _textMeshPro.text = FormatNumber(value);
                _animator.SetFloat("Health", 1 - (float)Math.Round(_hp / (double)_health, 2));
                if (_hp <= 0) Disappear();
            }
        }


        public void Reset()
        {
            _textMeshPro ??= GetComponentInChildren<TextMeshPro>();
            _animator = GetComponentInChildren<Animator>();
            _collider ??= GetComponentInChildren<MbCollider>();
            _collider.Enabled = true;
            _textMeshPro.enabled = true;
        }

        private void OnEnable()
        {
            _collider.TriggerEnter += OnHit;
        }

        private void OnDisable()
        {
            _collider.TriggerEnter -= OnHit;
        }

        private void OnHit(GameObject other)
        {
            if (!other.TryGetComponent(out MbBullet bullet)) return;
            Hp -= bullet.Damage;
        }

        private void Disappear()
        {
            _animator.SetTrigger("Break");
            _collider.Enabled = false;
            _textMeshPro.enabled = false;
            StartCoroutine(WaitForAnimationEnd());
        }

        private IEnumerator WaitForAnimationEnd()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
            MbTowerSpawner.Instance.Despawn(this);
        }

        private string FormatNumber(long number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }

    }
}

