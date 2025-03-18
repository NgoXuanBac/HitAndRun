using System;
using HitAndRun.Map;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbTeamMovement : MonoBehaviour
    {
        [SerializeField, Range(1, 10)] private float _moveSpeed = 2f;
        [SerializeField, Range(1, 20)] private float _forwardSpeed = 15f;
        [SerializeField] private MbGround _ground;
        public Action<Vector3> OnFinish;
        private float _targetX = 0f;

        public void Reset()
        {
            _ground = FindObjectOfType<MbGround>();
            _targetX = 0f;
        }

        private void Update()
        {
            if (transform.position.z >= _ground.Finish.z)
            {
                OnFinish?.Invoke(_ground.Finish);
                return;
            }
            transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime, Space.World);

            var touches = InputHelper.GetTouches();
            if (touches.Count == 0) return;
            var touch = touches[0];

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