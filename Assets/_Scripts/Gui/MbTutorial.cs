using UnityEngine;
using DG.Tweening;

namespace HitAndRun.Gui
{
    public class MbTutorial : MonoBehaviour
    {
        [SerializeField] private Transform _tap;
        [SerializeField] private float _duration = 1f;
        private Tween _moveTween;

        private void Reset()
        {
            _tap = transform.Find("Tap");
        }

        private void Start()
        {
            OnEnable();
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
        }

        private void OnEnable()
        {
            _moveTween = _tap
                .DOLocalMoveX(-1 * _tap.localPosition.x, _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
    }
}