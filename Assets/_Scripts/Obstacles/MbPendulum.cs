using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbPendulum : MbObstacle
    {
        [SerializeField]
        protected Transform _pendulum;
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        protected override void Reset()
        {
            _pendulum = transform.Find("Pendulum");
        }

        protected override void Start()
        {
            if (_pendulum == null)
            {
                Debug.LogError("Pendulum not found");
                return;
            }
            _pendulum.localEulerAngles = new Vector3(0, 0, -30);
            _pendulum.DORotate(new Vector3(0, 0, 30), 1 / _speed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}

