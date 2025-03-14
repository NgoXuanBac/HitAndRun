using System;
using System.Collections.Generic;
using HitAndRun.Enemy;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbEnemyTracker : MbSingleton<MbEnemyTracker>
    {
        [SerializeField] private List<MbEnemy> _enemies;
        public event Action OnEnemiesDied;

        private void Reset()
        {
            var enemies = FindObjectsOfType<MbEnemy>();

            foreach (var enemy in enemies)
            {
                _enemies.Add(enemy);
            }
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