using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbBlockWall : MbObstacle
    {
        [SerializeField]
        protected Transform _blockWall;
        [SerializeField, Range(0.1f, 1f)]
        protected float _speed = 0.5f;
        protected override void Reset()
        {
            _blockWall = transform.Find(Obstacles.BlockWall);
        }

        protected override void Start()
        {
            if (_blockWall == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(Obstacles.BlockWall)} not found");
#endif
                return;
            }
        }

        public void Fall()
        {
            _blockWall.DOLocalRotate(new Vector3(-90, 0, 0), 1 / _speed).SetEase(Ease.InExpo);
        }
    }
}

