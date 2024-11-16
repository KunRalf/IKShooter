using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponSys
{
    public abstract class RaycastWeapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponRecoil _weaponRecoil;
        [SerializeField] protected List<ParticleSystem> _muzzleFlash;
       
        [SerializeField] protected RaycastBullet _bullet;
        [SerializeField] protected Transform _raycastOrigin;
        [SerializeField][SyncVar] protected LookAtTarget _aimlook;
        [SerializeField] protected float _fireRate;
        [field:Header("IK")]
        [field:SerializeField] public Transform LHand { get; private set; }
        [field:SerializeField] public Transform RHand { get; private set; }

        public void Init(LookAtTarget aimLook)
        {
            _aimlook = aimLook;
        }

        public abstract void Shoot();

        public void Effects()
        {
            foreach (var muzzle in _muzzleFlash)
            {
                muzzle.Emit(1);
            }
        }
    }
}