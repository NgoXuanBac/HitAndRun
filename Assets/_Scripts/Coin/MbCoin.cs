using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Coin
{
    public class MbCoin : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        private void OnDisable()
        {
            transform.DOKill();
            transform.rotation = Quaternion.identity;
        }

        private void OnEnable()
        {
            transform.DORotate(new Vector3(0, 360, 0), 1 / _speed, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Character")) return;
            MbGameManager.Instance.AddCoin(1);
            MbCoinSpawner.Instance.Despawn(this);
        }
    }
}