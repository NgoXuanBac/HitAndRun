using HitAndRun.Inspector;
using UnityEngine;
namespace HitAndRun.Map
{
    public class MbGround : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Transform _road;
        [SerializeField] private Transform _finish;
        [SerializeField, ReadOnly] private int _chunkCount = 10;
        public Vector3 Finish => _finish.position;
        public int ChunkCount
        {
            get => _chunkCount;
            set
            {
                _chunkCount = value;
                _road.localScale = new Vector3(1, 1, _chunkCount);
                var length = _renderer.bounds.size.z;

                _finish.localPosition = new Vector3(0, -1f, _road.localPosition.z + length + 3);

                if (Application.isPlaying)
                    _renderer.material.mainTextureScale = new Vector2(1, _chunkCount);
                else
                    _renderer.sharedMaterial.mainTextureScale = new Vector2(1, _chunkCount);

            }
        }
        public float Width => _renderer.bounds.size.x;
        public float Length => _renderer.bounds.size.z;
        private void Reset()
        {
            _road = transform.Find("Road");
            _finish = transform.Find("Finish");

            _renderer = _road.Find("Model").GetComponentInChildren<MeshRenderer>();
            ChunkCount = _chunkCount;
        }
    }
}

