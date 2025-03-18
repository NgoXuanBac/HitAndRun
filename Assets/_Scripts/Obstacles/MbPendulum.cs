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
            _pendulum = transform.Find("Model").Find(Obstacles.Pendulum);
        }

        protected override void Start()
        {
            if (_pendulum == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(Obstacles.Pendulum)} not found");
#endif
                return;
            }
            _pendulum.localEulerAngles = new Vector3(0, 0, -30);
            _pendulum.DORotate(new Vector3(0, 0, 30), 1 / _speed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}

