using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbSpike : MbObstacle
    {
        [SerializeField]
        protected Transform _spike;
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        [SerializeField, Range(1f, 5f)]
        protected float _delay = 2f;
        protected override void Reset()
        {
            _spike = transform.Find(Obstacles.Spike);
        }

        protected override void Start()
        {
            if (_spike == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(Obstacles.Spike)} not found");
#endif
                return;
            }
            float height = _spike.GetComponent<Renderer>().bounds.size.y / 2;
            Sequence seq = DOTween.Sequence();
            seq.Append(_spike.DOShakePosition(1f, new Vector3(0, height * 0.05f, 0), 10, 90, false, true, ShakeRandomnessMode.Harmonic));
            seq.Append(_spike.DOMoveY(height, 1 / _speed).SetEase(Ease.OutExpo));
            seq.Append(_spike.DOMoveY(0, 1 / _speed).SetEase(Ease.InExpo));
            seq.AppendInterval(_delay);
            seq.SetLoops(-1, LoopType.Restart);
        }
    }
}

