using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponSys
{
    public class RaycastBullet : NetworkBehaviour
    {
        [SerializeField] private TrailRenderer _tracer;
        [SerializeField] protected float _bulletDrop = 0.0f;
        [SerializeField] protected float _bulletSpeed = 1000.0f;
        [SerializeField] protected List<ParticleSystem> _hitEffect;
        
        private float _timeBullet;
        private Vector3 _initialPosition;
        private Vector3 _initialVelocity;
        private Ray _ray;
        private float _maxLifeTime = 30.0f;
        private RaycastHit _hitInfo;

        public void Init(Vector3 initialPos, Vector3 initialVector)
        {
            _initialVelocity = initialVector * _bulletSpeed;
            _initialPosition = initialPos;
            _tracer.AddPosition(_initialPosition);
            DestroyBullet();
        }
        
        private void DestroyBullet()
        {
             Invoke(nameof(DestroySelf), _maxLifeTime);
        }
        
        [Server]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }
        private void SimulateBullet(float deltaTime)
        {
            Vector3 p0 = GetPosition(this);
            _timeBullet += deltaTime;
            Vector3 p1 = GetPosition(this);
            RaycastSegment(p0, p1);
        }
        
        private Vector3 GetPosition(RaycastBullet bullet)
        {
            Vector3 gravity = Vector3.down * _bulletDrop;
            return (bullet._initialPosition) + (bullet._initialVelocity * bullet._timeBullet + 0.5f * gravity * bullet._timeBullet * bullet._timeBullet);
        }
        
        private void RaycastSegment(Vector3 start, Vector3 end)
        {
            float distance = Vector3.Distance(start, end);
            Vector3 direction = (end - start).normalized;
            _ray.origin = start;
            _ray.direction = direction;


            if (Physics.Raycast(_ray, out _hitInfo, distance))
            {
                foreach (ParticleSystem hit in _hitEffect)
                {
                    hit.transform.position = _hitInfo.point;
                    hit.transform.forward = _hitInfo.normal;
                    hit.Emit(1);
                        
                    _tracer.transform.position = _hitInfo.point;
                    _timeBullet = _maxLifeTime;
                }

                if(_hitInfo.transform.GetComponent<PlayerHealth>() != null)
                {
                    _hitInfo.transform.GetComponent<PlayerHealth>().TakeDamage(15);
                }
            }
            else
            {
                if(_tracer != null)
                    _tracer.transform.position = end;
            }
        }

        private void Update()
        {
            SimulateBullet(Time.deltaTime);
        }
    }
}