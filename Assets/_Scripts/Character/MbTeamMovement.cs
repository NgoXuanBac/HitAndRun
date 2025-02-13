using UnityEngine;

namespace HitAndRun.Character
{
    public class MbTeamMovement : MonoBehaviour
    {
        [SerializeField, Range(1, 10)] private float _moveSpeed = 5f;
        [SerializeField, Range(1, 20)] private float _forwardSpeed = 8f;
        private bool _hasStarted = false;
        private float _targetX;
        public void Reset() => _hasStarted = false;
        private void Update()
        {
            var touches = InputHelper.GetTouches();
            if (_hasStarted)
                transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime, Space.World);

            if (touches.Count == 0) return;
            var touch = touches[0];

            if (!_hasStarted && touch.phase == TouchPhase.Began)
                _hasStarted = true;

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