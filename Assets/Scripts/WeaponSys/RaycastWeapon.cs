using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponSys
{
    class Bullet1
    {
        public float Time;
        public Vector3 InitialPosition;
        public Vector3 InitialVelocity;
        public TrailRenderer Tracer;
    }
    
    public class RaycastWeapon : NetworkBehaviour
    {
         [SerializeField] private WeaponRecoil _recoil;
        // public ActiveWeapon.WeaponSlots weaponSlot;

        [SerializeField] private List<ParticleSystem> _muzzleFlash;
        [SerializeField] private List<ParticleSystem> _hitEffect;
        [SerializeField] private TrailRenderer _tracerEffect;
        
        public bool IsFireing;

        [SerializeField] private Transform _raycastOrigin;
        // [SerializeField] private Transform raycastDestination;
        [SyncVar(hook = nameof(SetLook))] private GameObject _aimLooGO;
        [SerializeField] private Transform _aimlookT;
        private Ray _ray;

        private float _maxLifeTime = 30.0f;
        private RaycastHit _hitInfo;
        private float _acumalatedTime;
        [SerializeField] private float _fireRate;

        [SerializeField] private float _bulletSpeed = 1000.0f;
        [SerializeField] private float _bulletDrop = 0.0f;

        [SerializeField] private Transform _capsuleInstantiatePosition;
        private List<Bullet1> _bullets = new List<Bullet1>();
        void Awake()
        {
          // / recoil = GetComponent<WeaponRecoil>();
        }

        
        Vector3 GetPosition(Bullet1 bullet)
        {
            Vector3 gravity = Vector3.down * _bulletDrop;
            return (bullet.InitialPosition) + (bullet.InitialVelocity * bullet.Time + 0.5f * gravity * bullet.Time * bullet.Time);
        }

        public void Init(GameObject look)
        {
            _aimLooGO = look;
        }
        
        private void SetLook(GameObject o, GameObject n)
        {
            _aimlookT = n.transform;
        }
        
        Bullet1 CreateBullet(Vector3 initialPosition, Vector3 initialVelocity)
        {
            Bullet1 bullet = new Bullet1();
            bullet.InitialPosition = initialPosition;
            bullet.InitialVelocity = initialVelocity;
            bullet.Time = 0.0f;
            var a = Instantiate(_tracerEffect, initialPosition, Quaternion.identity);
            NetworkServer.Spawn(a.gameObject, connectionToClient);
            bullet.Tracer = a;
            bullet.Tracer.AddPosition(initialPosition);
            return bullet;
        }

        public void UpdateBullets( float deltaTime)
        {
            SimulateBullet(deltaTime);
            DestroyBullet();
        }

        void DestroyBullet()
        {
            _bullets.RemoveAll(bullet => bullet.Time > _maxLifeTime);
        }

        void SimulateBullet(float deltaTime)
        {
            _bullets.ForEach(bullet =>
            {
                Vector3 p0 = GetPosition(bullet);
                bullet.Time += deltaTime;
                Vector3 p1  = GetPosition(bullet);
                RaycastSegment(p0, p1, bullet);
            });
        }

        void RaycastSegment(Vector3 start, Vector3 end, Bullet1 bullet)
        {
            float distance = Vector3.Distance(start, end);
            Vector3 direction = (end - start).normalized;
            _ray.origin = start;
            _ray.direction = direction;


            if (Physics.Raycast(_ray, out _hitInfo, distance))
            {
                //Debug.Log(hitInfo.transform.name);
                if(_hitInfo.transform.tag != "Player")
                {
                    foreach (ParticleSystem hit in _hitEffect)
                    {
                        hit.transform.position = _hitInfo.point;
                        hit.transform.forward = _hitInfo.normal;
                        hit.Emit(1);
                        
                        bullet.Tracer.transform.position = _hitInfo.point;
                        bullet.Time = _maxLifeTime;
                    }

                    if(_hitInfo.transform.GetComponent<PlayerHealth>() != null)
                    {
                        _hitInfo.transform.GetComponent<PlayerHealth>().TakeDamage(15);
                    }
                }
            }
            else
            {
                if(bullet.Tracer != null)
                    bullet.Tracer.transform.position = end;
            }
        }

        public void StartFireing()
        {
            _acumalatedTime = 0.0f;
            IsFireing = true;
            FireBullet();
        }

        public void UpdateFireing(float deltaTime)
        {
            _acumalatedTime += deltaTime;
            float fireInterval  = 1.0f / _fireRate;
            while (_acumalatedTime > 0.1f)
            {
                FireBullet();
                _acumalatedTime -= fireInterval;
            }
        }

        private void FireBullet()
        {
            foreach (var muzzle in _muzzleFlash)
            {
                muzzle.Emit(1);
            }
            Vector3 velocity = (_aimlookT.position - _raycastOrigin.position).normalized * _bulletSpeed;
            var bullet = CreateBullet(_raycastOrigin.position, velocity);
            _bullets.Add(bullet);
            // recoil.GenerateRecoil(weaponName);
            
        }

        public void StopFireing()
        {
            IsFireing = false;
        }
    }
}