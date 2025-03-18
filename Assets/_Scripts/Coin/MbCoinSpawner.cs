using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Coin
{
    public class MbCoinSpawner : MbSingleton<MbCoinSpawner>
    {
        [SerializeField] private MbCoin _prefab;
        private ConcurrentQueue<MbCoin> _pools = new();
        private void Reset()
        {
            _prefab = Resources.Load<MbCoin>("Prefabs/Coin");
        }

        public void SpawnGrid(Vector3 position, int row, int col)
        {
            var offsetX = (row - 1) * 2 / 2f;
            var offsetZ = (col - 1) * 2 / 2f;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var pos = position + new Vector3(i * 2 - offsetX, 0, j * 2 - offsetZ);
                    Spawn(pos, transform);
                }
            }
        }

        public MbCoin Spawn(Vector3 position, Transform parent)
        {
            if (_pools.Count == 0)
            {
                var newCoin = Instantiate(_prefab, transform);
                newCoin.gameObject.SetActive(false);
                _pools.Enqueue(newCoin);
            }

            _pools.TryDequeue(out var coin);
            coin.transform.position = position;
            coin.transform.parent = parent;
            coin.gameObject.SetActive(true);
            return coin;
        }

        public void Despawn(MbCoin coin)
        {
            coin.transform.SetParent(transform);
            coin.gameObject.SetActive(false);
            _pools.Enqueue(coin);
        }
    }
}


