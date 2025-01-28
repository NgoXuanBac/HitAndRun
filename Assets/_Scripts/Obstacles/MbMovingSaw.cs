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
            _base = transform.Find("Base");

        }

        protected override void Start()
        {
            base.Start();
            if (_base == null)
            {
                Debug.LogError("Base not found");
                return;
            }
            var width = _base.GetComponent<Renderer>().bounds.size.x;
            var offset = _saw.GetComponent<Renderer>().bounds.size.x / 2;
            var start = _base.position - _base.right * (width / 2 - offset);
            var end = _base.position + _base.right * (width / 2 - offset);
            _saw.position = start;

            _saw.DOMove(end, 1 / _speed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}

