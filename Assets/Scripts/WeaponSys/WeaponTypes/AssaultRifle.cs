using Mirror;
using UnityEngine;
using WeaponSys.Bullets;

namespace WeaponSys.WeaponTypes
{
    public class AssaultRifle : Weapon
    {
        public override void Shoot()
        {
            if (!CanShoot()) return;
            Bullet bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            NetworkServer.Spawn(bullet.gameObject, connectionToClient);
            _currentAmmo--;
            _recoil.GenerateRecoil();
            UpdateFireCooldown();
        }

        private Vector3 GetSpreadDirection()
        {
            Vector3 direction = transform.forward;
            direction.x += Random.Range(-_weaponData.BulletSpread, _weaponData.BulletSpread);
            direction.y += Random.Range(-_weaponData.BulletSpread, _weaponData.BulletSpread);
            return direction.normalized;
        }
    }
}