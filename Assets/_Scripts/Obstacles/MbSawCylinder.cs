using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbSawCylinder : MbObstacle
    {
        [SerializeField]
        protected Transform _saw;
        [SerializeField]
        private Transform _pivot;
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        protected override void Reset()
        {
            _saw = transform.Find("Saw");
            _pivot = transform.Find("Pivot");
        }

        protected override void Start()
        {
            if (_saw == null)
            {
                Debug.LogError("Saw not found");
                return;
            }

            _saw.DOLocalRotate(new(0, 360, 0), 1 / _speed, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
            _saw.SetParent(_pivot);
            _pivot.DORotate(new(0, 360, 0), 1 / _speed, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
        }
    }

}

