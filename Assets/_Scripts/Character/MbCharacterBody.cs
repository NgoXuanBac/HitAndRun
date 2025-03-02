using System;
using HitAndRun.Inspector;
using TMPro;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbCharacterBody : MonoBehaviour
    {
        [SerializeField, ReadOnly, Min(2)] private int _level;
        [SerializeField, Range(1, 100)] private int _gravityScale = 20;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private SOBodyTypes _bodyTypes;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _platformLM;
        [SerializeField, Range(0, 1)] private float _scaleUp = 0.02f;
        [SerializeField, ReadOnly] private float _radius;
        public float Width => _radius * transform.localScale.x * 2;
        [SerializeField, ReadOnly] private Color _color;

        private bool _isGrounded = true;
        public bool IsGrounded => _isGrounded;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                if (!Application.isPlaying) return;
                _meshRenderer.material.SetColor("_BaseColor", _color);
            }
        }
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                _textMeshPro.text = FormatNumber(value);
                Color = _bodyTypes.GetColor(_level);
                transform.localScale = Vector3.one + (Mathf.Log(_level, 2) - 1) * _scaleUp * Vector3.one;
            }
        }

        public void Reset()
        {
            _bodyTypes ??= Resources.Load<SOBodyTypes>("Scriptables/BodyTypes");
            _collider ??= GetComponent<CapsuleCollider>();
            _rigidbody ??= GetComponent<Rigidbody>();
            _textMeshPro ??= GetComponentInChildren<TextMeshPro>();
            _meshRenderer ??= GetComponentInChildren<SkinnedMeshRenderer>();

            _radius = GetComponent<CapsuleCollider>().radius;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.Sleep();
            Level = 2;
        }

        private void Update()
        {
            _isGrounded = CheckGrounded();
        }

        private void FixedUpdate()
        {
            if (!_isGrounded) _rigidbody.AddForce(Physics.gravity.y * _gravityScale * Vector3.up, ForceMode.Acceleration);
            else _rigidbody.velocity = Vector3.zero;
        }

        private bool CheckGrounded()
        {
            var offset = 0.5f;
            var bounds = _collider.bounds;
            var hit = Physics.BoxCast(bounds.center, bounds.extents, Vector3.down, Quaternion.identity, offset, _platformLM);
#if UNITY_EDITOR
            var color = hit ? Color.green : Color.red;
            Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x, 0, 0), Vector3.down * (bounds.extents.y + offset), color);
            Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, 0, 0), Vector3.down * (bounds.extents.y + offset), color);
            Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, bounds.extents.y + offset, 0), Vector3.right * (bounds.extents.x * 2f), color);
#endif
            return hit;
        }


        private string FormatNumber(int number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }
    }
}



