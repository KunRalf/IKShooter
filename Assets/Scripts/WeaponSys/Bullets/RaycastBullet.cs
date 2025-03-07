﻿using System.Collections;
using System.Runtime.CompilerServices;
using Interfaces;
using Mirror;
using Player;
using UnityEngine;

namespace WeaponSys.Bullets
{
    public class RaycastBullet : NetworkBehaviour
    {
        [SerializeField] protected BulletData _bulletData;
        [SerializeField] private TrailRenderer _tracer;
        [SerializeField] protected ParticleSystem _hitEffect;
        
        private float _timeBullet;
        private Vector3 _initialPosition;
        private Vector3 _initialVelocity;
        private Ray _ray;
        private RaycastHit _hitInfo;
        private IEnumerator _destroyBullet;
        private bool _isStopBullet;

        public void Init(Vector3 initialPos, Vector3 initialVector)
        {
            _initialVelocity = initialVector * _bulletData.InitialSpeed;
            _initialPosition = initialPos;
            _tracer.AddPosition(_initialPosition);
            _destroyBullet =  DestroyBullet();
            _tracer.time = _bulletData.Lifetime;
            StartCoroutine(_destroyBullet);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_bulletData.Lifetime);
            DestroySelf();
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
            RpcUpdateTracer(p1);
        }
        
        private Vector3 GetPosition(RaycastBullet bullet)
        {
            Vector3 gravity = Vector3.down * _bulletData.BulletDropSpeed;
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
                if(_hitInfo.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_bulletData.Damage);
                    DestroySelf();
                }
                ShowHitEffect(_hitInfo.point, _hitInfo.normal);
                _isStopBullet = true;
                StopCoroutine(_destroyBullet);
                StartCoroutine(DestroyBullet());
            }
            else
            {
                if(_tracer != null)
                    _tracer.transform.position = end;
            }
        }

        [ClientRpc]
        private void RpcUpdateTracer(Vector3 newPosition)
        {
            if (_tracer != null)
                _tracer.transform.position = newPosition;
        }
        
        [ClientRpc]
        private void ShowHitEffect(Vector3 a, Vector3 b)
        {
            _hitEffect.gameObject.SetActive(true);
            _hitEffect.transform.position = a;
            _hitEffect.transform.forward = b;
            _hitEffect.Emit(1);
        }
        [ServerCallback]
        private void Update()
        {
            if(_isStopBullet) return;
            SimulateBullet(Time.deltaTime);
        }
    }
}