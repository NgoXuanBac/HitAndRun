using System;
using System.Collections.Generic;
using System.Linq;
using HitAndRun.Inspector;
using UnityEngine;
namespace HitAndRun.Map
{
    public class MbGround : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Transform _road;
        [SerializeField] private Transform _finish;
        [SerializeField] private Transform _enemy;
        [SerializeField, ReadOnly] private int _chunkCount = 10;
        [SerializeField] private List<Enviroment> _enviroments = new();
        [SerializeField] private Texture2D _stage;
        public Vector3 Finish => _finish.position;
        public Vector3 Enemy => _enemy.position;
        public int ChunkCount => _chunkCount;
        public float Width => _renderer.bounds.size.x;
        public float Length => _renderer.bounds.size.z;
        private void Reset()
        {
            _road = transform.Find("Road");
            _finish = transform.Find("Finish");
            _enemy = GameObject.Find("Enemy").transform;

            _renderer = _road.Find("Model").GetComponentInChildren<MeshRenderer>();
            var stages = Resources.LoadAll<Texture2D>("Textures/Stage").ToList();

            foreach (var stage in stages)
            {
                _enviroments.Add(new Enviroment { Stage = stage });
            }
        }

        public void Setup(int chunkCount)
        {
            _chunkCount = chunkCount;

            _road.localScale = new Vector3(1, 1, _chunkCount);
            var length = _renderer.bounds.size.z;

            _finish.localPosition = new Vector3(0, -1f, _road.localPosition.z + length + 3);


            var avaiables = _enviroments.Where(env => env.Stage != _stage).ToList();
            var enviroment = avaiables.Count > 0 ? avaiables[UnityEngine.Random.Range(0, avaiables.Count)] : _enviroments[0];

            _stage = enviroment.Stage;
            if (Application.isPlaying)
            {
                _renderer.material.SetTexture("_BaseMap", enviroment.Stage);
                _renderer.material.mainTextureScale = new Vector2(1, _chunkCount);
            }
            else
            {
                _renderer.sharedMaterial.SetTexture("_BaseMap", enviroment.Stage);
                _renderer.sharedMaterial.mainTextureScale = new Vector2(1, _chunkCount);
            }

            if (enviroment.Skybox != null)
            {
                RenderSettings.skybox = enviroment.Skybox;
                DynamicGI.UpdateEnvironment();
            }
        }

        [Serializable]
        private struct Enviroment
        {
            public Texture2D Stage;
            public Material Skybox;
        }
    }
}

