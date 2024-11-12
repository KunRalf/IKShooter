using Mirror;
using UnityEngine;

namespace WeaponSys.WeaponTypes
{
    public class RaycastAssaultRifle : RaycastWeapon
    {
        public override void Shoot()
        {
            var bullet = Instantiate(_tracerEffect, _raycastOrigin.position, Quaternion.identity);
            NetworkServer.Spawn(bullet.gameObject, connectionToClient);
            bullet.Init(_raycastOrigin.position, (_aimlookT.position - _raycastOrigin.position).normalized);
        }
    }
}