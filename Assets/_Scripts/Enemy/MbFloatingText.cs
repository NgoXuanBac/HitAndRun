using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HitAndRun.Enemy
{
    public class MbFloatingText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private float _moveDistance = 2f;
        [SerializeField] private float _duration = 0.5f;

        private void Reset()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        public void Popup(string text, float scale = 0.5f)
        {
            _textMesh.text = text;
            _textMesh.alpha = 1;

            var randomDirection = new Vector3(
                Random.Range(-_moveDistance, _moveDistance),
                _moveDistance,
                0
            );
            transform.localScale = Vector3.one * 0.5f;
            transform.DOScale(scale, _duration).SetEase(Ease.OutBack);

            transform.DOMove(transform.position + randomDirection, _duration).SetEase(Ease.OutCubic);

            _textMesh.DOFade(0, _duration).SetEase(Ease.InCubic)
                .OnComplete(() => MbFloatingTextSpawner.Instance.Despawn(this));
        }
    }
}

