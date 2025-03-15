using System;
using System.Collections.Generic;
using System.Linq;
using HitAndRun.Enemy;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbEnemyTracker : MbSingleton<MbEnemyTracker>
    {
        [SerializeField] private List<MbEnemy> _enemies = new();
        public List<MbEnemy> Enemies => _enemies;
        public event Action OnEnemiesDied;

        public void Reset()
        {
            _enemies?.Clear();
            _enemies = FindObjectsOfType<MbEnemy>().OrderBy(e => e.transform.position.x).ToList();
        }

        public void RemoveEnemy(MbEnemy enemy)
        {
            if (_enemies.Contains(enemy)) _enemies.Remove(enemy);
            if (_enemies.Count == 0)
            {
                OnEnemiesDied?.Invoke();
            }
        }

    }
}