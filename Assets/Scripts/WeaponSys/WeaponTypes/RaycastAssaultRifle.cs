using Mirror;
using UnityEngine;

namespace WeaponSys.WeaponTypes
{
    public class RaycastAssaultRifle : RaycastWeapon
    {
        public override void Shoot()
        {
            var bullet = Instantiate(_bullet, _raycastOrigin.position, Quaternion.identity);
            NetworkServer.Spawn(bullet.gameObject, connectionToClient);
            bullet.Init(_raycastOrigin.position, (_aimlook.position - _raycastOrigin.position).normalized);
            foreach (var muzzle in _muzzleFlash)
            {
                muzzle.Emit(1);
            }
        }
    }
}