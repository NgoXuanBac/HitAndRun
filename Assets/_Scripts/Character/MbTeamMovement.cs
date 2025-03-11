using System;
using HitAndRun.Map;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbTeamMovement : MonoBehaviour
    {
        [SerializeField, Range(1, 10)] private float _moveSpeed = 2f;
        [SerializeField, Range(1, 20)] private float _forwardSpeed = 15f;
        public bool Stop { get; set; } = true;
        private float _targetX;
        [SerializeField]
        private MbGround _ground;
        public Action<Vector3> OnFinish;
        public Action OnMove;
        private bool _hasTouchedOnce = false;
        private bool _isFinish = false;

        public void Reset()
        {
            _ground = FindObjectOfType<MbGround>();
            _hasTouchedOnce = false;
            _isFinish = false;
            Stop = true;
        }

        private void Awake()
        {
            OnFinish += (_) => _isFinish = true;
        }


        private void Update()
        {
            if (_isFinish) return;

            var touches = InputHelper.GetTouches();
            if (!Stop)
            {
                if (transform.position.z >= _ground.Finish.z)
                {
                    Stop = true;
                    OnFinish?.Invoke(_ground.Finish);
                    return;
                }
                transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime, Space.World);
            }

            if (touches.Count == 0) return;
            var touch = touches[0];

            if (Stop && touch.phase == TouchPhase.Began)
            {
                Stop = false;
                if (!_hasTouchedOnce)
                {
                    _hasTouchedOnce = true;
                    OnMove?.Invoke();
                }
            }
            if (touch.phase == TouchPhase.Moved)
                _targetX += touch.deltaPosition.x * _moveSpeed * Time.deltaTime;

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, _targetX, 0.1f),
                transform.position.y,
                transform.position.z
            );
        }
    }
}