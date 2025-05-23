using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbHammer : MbObstacle
    {
        [SerializeField]
        protected Transform _hammer;
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        [SerializeField, Range(0.1f, 2f)]
        protected float _delay = 0.5f;

        protected override void OnDisable()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEnable()
        {
            throw new System.NotImplementedException();
        }

        protected override void Reset()
        {
            _hammer = transform.Find("Model").Find(Obstacles.Hammer);
        }

        protected override void Start()
        {
            if (_hammer == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(Obstacles.Hammer)} not found");
#endif
                return;
            }
            Sequence seq = DOTween.Sequence();
            seq.Append(_hammer.DOLocalRotate(new Vector3(-90, 0, 0), 1 / _speed).SetEase(Ease.InExpo));
            seq.AppendInterval(_delay);
            seq.SetLoops(-1, LoopType.Yoyo);
        }
    }
}

