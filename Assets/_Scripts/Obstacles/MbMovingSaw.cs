using DG.Tweening;
using UnityEngine;
namespace HitAndRun.Obstacles
{
    public class MbMovingSaw : MbSaw
    {
        [SerializeField]
        private Transform _base;
        protected override Vector3 Rotate => new(0, 360, 0);
        protected override void Reset()
        {
            base.Reset();
            _base = transform.Find("Model").Find("Base");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_base == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Base not found");
#endif
                return;
            }
            var start = _base.localPosition - _base.right * 0.5f;
            var end = _base.localPosition + _base.right * 0.5f;
            _saw.localPosition = start;

            _saw.DOLocalMove(end, 1 / _speed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}

