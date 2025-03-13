using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbCharacterSpawner : MbSingleton<MbCharacterSpawner>
    {
        [SerializeField] private MbCharacter _prefab;
        private ConcurrentQueue<MbCharacter> _pools = new();

        private void Reset()
        {
            _prefab = Resources.Load<MbCharacter>("Prefabs/Character");
        }

        public MbCharacter Spawn(Vector3 position, Transform parent, bool isActive = false)
        {
            if (_pools.Count == 0)
            {
                var newCharacter = Instantiate(_prefab, parent);
                newCharacter.gameObject.SetActive(false);
                _pools.Enqueue(newCharacter);
            }
            _pools.TryDequeue(out var character);
            character.name = $"Character#{character.GetHashCode()}";
            character.transform.position = position;
            character.gameObject.SetActive(true);
            character.IsActive = isActive;
            return character;
        }

        public void Despawn(MbCharacter character)
        {
            character.Reset();
            character.transform.SetParent(transform);
            character.gameObject.SetActive(false);
            _pools.Enqueue(character);
        }
    }

}
