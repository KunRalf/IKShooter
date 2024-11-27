using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSys.Bullets;
using WeaponSys.WeaponTypes;

namespace WeaponSys
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] protected WeaponRecoil _weaponRecoil;
        [SerializeField] protected List<ParticleSystem> _muzzleFlash;
        [SerializeField] protected WeaponData _weaponData;
       
        [SerializeField] protected RaycastBullet _bullet;
        [SerializeField] protected Transform _raycastOrigin;
        [SerializeField][SyncVar] protected Transform _aimlook;
        [SerializeField] protected float _fireRate;
        [field:Header("IK")]
        [field:SerializeField] public Transform LHand { get; private set; }
        [field:SerializeField] public Transform RHand { get; private set; }

        private float _delayToFire = 0f;
        [SyncVar(hook = nameof(SyncAmmo))] protected int _currentAmmo;

        public event Action<int> OnUpdateAmmo;
        
        public void Init(GameObject aimLook)
        {
            _aimlook = aimLook.transform;
        }

        private void Start()
        {
            _currentAmmo = _weaponData.MaxAmmo;
        }

        public abstract void Shoot();

        public void Effects()
        {
            foreach (var muzzle in _muzzleFlash)
            {
                muzzle.Emit(1);
            }
        }
        
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

        private void SyncAmmo(int o, int n)
        {
            OnUpdateAmmo?.Invoke(n);
        }

        public virtual void Reload()
        {
            _currentAmmo = _weaponData.MaxAmmo;
            Debug.Log($"{_weaponData.WeaponName} reloaded!");
        }
    }
}