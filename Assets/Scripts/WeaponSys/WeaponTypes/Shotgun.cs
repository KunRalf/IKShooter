using Mirror;
using UnityEngine;
using WeaponSys.Bullets;

namespace WeaponSys.WeaponTypes
{
    public class Shotgun : Weapon
    {
        public override void Shoot()
        {
            if (!CanShoot()) return;
            
       
            for (int i = 0; i < _weaponData.PelletCount; i++)
            {
                var bullet = Instantiate(_bullet, _raycastOrigin.position, Quaternion.identity);
                NetworkServer.Spawn(bullet.gameObject, connectionToClient);
                bullet.Init(_raycastOrigin.position, (_aimlook.position - _raycastOrigin.position).normalized);
            }

            _currentAmmo--;
            UpdateFireCooldown();
        }
        
    
    }
}