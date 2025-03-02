using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HitAndRun.Gate.Player;
using TMPro;

namespace HitAndRun.Gate.Player{
    public class MbPlayerCrowd : MonoBehaviour
    {
        [SerializeField] private int crowdSizeForDebug = 5;
        [SerializeField] private int startingCrowdSize = 1;

        [SerializeField] private MbPlayerShooter shooterPrefab;
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        private List<MbPlayerShooter> _shooters = new List<MbPlayerShooter>();
        public List<MbPlayerShooter> Shooters => _shooters;
        [ContextMenu("Set")]
        public void Debug_ResizeCrowd() => Set(crowdSizeForDebug);

        [SerializeField] private TMP_Text yearText;
        [SerializeField] private TMP_Text damageText;

        private int _damage;
        private int _fireRate;

        private void Start()
        {
            Set(startingCrowdSize);
            damageText.text = _damage.ToString();
        }

        public void UpgradeDamageToCrowd(int damageToAdd)
        {
            _damage += damageToAdd;
            foreach (var shooter in _shooters)      
            {
                shooter.UpgradeDamage(damageToAdd);
            }
            damageText.text = _damage.ToString();
        }

        public void UpgradeFireRateToCrowd(int fireRateToAdd)
        {
            _fireRate *= fireRateToAdd;
            foreach (var shooter in _shooters)      
            {
                shooter.UpgradeFireRate(fireRateToAdd);
            }
            damageText.text = _damage.ToString();
        }
        public void Set(int amount)
        {
            if (_shooters.Count == amount) return;
            var needToRemove = amount < _shooters.Count;
            var needToAdd = amount > _shooters.Count;
            while (amount != _shooters.Count)
            {
                if(needToRemove) RemoveShooter();
                else if (needToAdd) AddShooter();
            }
        }

        public bool CanAdd()
        {
            return _shooters.Count + 1 <= spawnPoints.Count;
        }

        public bool CanRemove()
        {
            return _shooters.Count - 2 >= 0;
        }
        public void RemoveShooter()
        {
            if (!CanRemove()) return;
            var lastShooter = _shooters[_shooters.Count - 1];
            _shooters.Remove(lastShooter);
            Destroy(lastShooter.gameObject);
        }

        public void AddShooter()
        {
            if (!CanAdd()) return;
            var lastShooterIndex = _shooters.Count - 1;
            var position = spawnPoints[lastShooterIndex + 1].position;
            var shooter = Instantiate(shooterPrefab, position, Quaternion.identity, transform);
            _shooters.Add(shooter);
        }
    }
}
