using UnityEngine;
using DG.Tweening;

namespace HitAndRun.Obstacles
{
    public class MbSaw : MbObstacle
    {
        [SerializeField]
        protected Transform _saw;

        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        protected virtual Vector3 Rotate => new(0, 0, 360);
        protected override void Reset()
        {
            _saw = transform.Find("Model").Find(Obstacles.Saw);
        }

        protected override void OnEnable()
        {
            if (_saw == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(Obstacles.Saw)} not found");
#endif
                return;
            }

            _saw.DORotate(Rotate, 1 / _speed, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
        }

        protected override void OnDisable()
        {
            _saw?.DOKill();
            _saw.rotation = Quaternion.identity;
        }
    }
}

