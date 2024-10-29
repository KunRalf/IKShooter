using UnityEngine;
using WeaponSys.Bullets;

namespace WeaponSys
{
    public abstract class Weapon : MonoBehaviour
    {
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

        public virtual void Reload()
        {
            _currentAmmo = _weaponData.MaxAmmo;
            Debug.Log($"{_weaponData.WeaponName} reloaded!");
        }
    }
}