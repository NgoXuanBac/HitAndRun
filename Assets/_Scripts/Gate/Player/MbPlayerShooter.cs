using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HitAndRun.Gate.Player{
    public class MbPlayerShooter : MonoBehaviour
    {
        [SerializeField] private float defaultShootDelay = 1f;
        [SerializeField] private float damagePerShootable = 100f;
        [SerializeField] private float fireRatePerShootable = 100f;
        // [SerializeField] private Shootable shootablePrefab;
        // [SerializeField] private Transform shootFrom;
        [SerializeField] private float damageAddPerGate = 2f;
        [SerializeField] private float fireRateAddPerGate = 2f;
        private float _shootDelay;
        private float _runningTimer;
        private float _damagePerShootable;
        private float _fireRatePerShootable;
        private int _damage = 0;
        private int _fireRate = 1;
        private int _ammo = 1;
        private void Start()
        {
            _shootDelay = defaultShootDelay;
            _damagePerShootable = damagePerShootable;
        }

        private void Update()
        {
            _runningTimer += Time.deltaTime;
            if (_runningTimer >= _shootDelay)
            {
                _runningTimer = 0f;
                // Shoot();
            }
        }

        public void UpgradeDamage(int toAdd)
        {
            _damage += toAdd;
            var damageToAdd = _damage * damageAddPerGate;
            _damagePerShootable = damagePerShootable + damageToAdd;
        }

        public void UpgradeFireRate(int toAdd)
        {
            if (toAdd > 0)
            {
                _fireRate *= toAdd;  
            }
            else if (toAdd < 0)
            {
                _fireRate /= Mathf.Abs(toAdd);  // Chia nếu toAdd là âm (chia đôi)
            }
    
            var fireRateToAdd = _fireRate * fireRateAddPerGate;
    
            _fireRatePerShootable = fireRatePerShootable * fireRateToAdd;

            Debug.Log($"Fire rate upgraded: {_fireRatePerShootable}"); 
        }
        
        public void UpgradeAmmo(int multiplier)
        {
            _ammo *= multiplier; 
            Debug.Log($"Ammo upgraded: {_ammo}"); 
        }
        // public void Shoot()
        // {
        //     Instantiate(shootablePrefab, shootFrom.position, Quaternion.identity).Init(_damagePerShootable);
        // }
    }
}
