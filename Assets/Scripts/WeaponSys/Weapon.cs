using System;
using UnityEngine;
using WeaponSys.Bullets;
using WeaponSys.WeaponTypes;

namespace WeaponSys
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected Bullet _bullet;
        [SerializeField] protected Transform _shootPoint;
        protected float _delayToFire = 0f;
        protected int _currentAmmo;

        protected virtual void Start()
        {
            _currentAmmo = _weaponData.MaxAmmo;
        }

        public abstract void Shoot();

        protected bool CanShoot()
        {
            return Time.time >= _delayToFire && _currentAmmo > 0;
        }

        protected void UpdateFireCooldown()
        {
            _delayToFire = Time.time + _weaponData.FireRate;
        }

        private void OnValidate()
        {
            if (_weaponData != null && (_weaponData.WeaponType != _weaponType))
            {
                Debug.LogError("Wrong weapon type");
                _weaponData = null;
            }
        }

        public virtual void Reload()
        {
            _currentAmmo = _weaponData.MaxAmmo;
            Debug.Log($"{_weaponData.WeaponName} reloaded!");
        }
    }
}