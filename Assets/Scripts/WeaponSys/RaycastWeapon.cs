using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace WeaponSys
{
    public abstract class RaycastWeapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponRecoil _weaponRecoil;
        [SerializeField] protected List<ParticleSystem> _muzzleFlash;
       
        [SerializeField] protected RaycastBullet _tracerEffect;
        [SerializeField] protected Transform _raycastOrigin;
        [SyncVar(hook = nameof(SetLook))] protected GameObject _aimLooGO;
        [SerializeField] protected Transform _aimlookT;
        [SerializeField] protected float _fireRate;

        public void Init(GameObject aimLook)
        {
            _aimLooGO = aimLook;
        }
        
        private void SetLook(GameObject o, GameObject n)
        {
            _aimlookT = n.transform;
        }

        public abstract void Shoot();
    }
}