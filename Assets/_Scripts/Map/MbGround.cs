using HitAndRun.Inspector;
using UnityEngine;
namespace HitAndRun.Map
{
    public class MbGround : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField, ReadOnly] private int _chunkCount = 1;
        public int ChunkCount
        {
            get => _chunkCount;
            set
            {
                _chunkCount = value;
                transform.localScale = new Vector3(1, 1, _chunkCount);
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
            _renderer = GetComponentInChildren<MeshRenderer>();
            ChunkCount = _chunkCount;
        }
    }
}

