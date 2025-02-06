using UnityEngine;

namespace HitAndRun.Character
{
    public class MbGroupMovement : MonoBehaviour
    {
        [SerializeField, Range(1, 10)] private float _moveSpeed = 5f;
        private float _target;

        private void Update()
        {
            var touches = InputHelper.GetTouches();

            if (touches.Count == 0) return;
            var touch = touches[0];

            if (touch.phase == TouchPhase.Moved)
                _target += touch.deltaPosition.x * _moveSpeed * Time.deltaTime;

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, _target, 0.1f), transform.position.y, transform.position.z);
        }
    }
}

