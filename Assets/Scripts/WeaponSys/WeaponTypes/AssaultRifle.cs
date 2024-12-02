using Mirror;
using UnityEngine;

namespace WeaponSys.WeaponTypes
{
    public class AssaultRifle : Weapon
    {
        public override void Shoot()
        {
            if(!CanShoot())return;
            var bullet = Instantiate(_bullet, _raycastOrigin.position, Quaternion.identity);
            NetworkServer.Spawn(bullet.gameObject, connectionToClient);
            bullet.Init(_raycastOrigin.position, (_aimlook.position - _raycastOrigin.position).normalized);
            _currentAmmo--;
            _weaponRecoil.GenerateRecoil();
           UpdateFireCooldown();
        }
    }
}