using System;
using Mirror;
using Player;
using UnityEngine;

namespace WeaponSys.Bullets
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private BulletData _bulletData; // ScriptableObject с параметрами пули
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), _bulletData.Lifetime);
        }

        private void Start()
        {
            _rb.AddForce(transform.forward * _bulletData.InitialSpeed, ForceMode.Impulse);
        }

        [Server]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }
        
        [ServerCallback]
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.TryGetComponent(out PlayerHealth player))
            {
                player.TakeDamage(_bulletData.Damage);
                DestroySelf();
                Debug.Log("Ударилось");
            }
            Debug.Log("Ударилось в стену");    
        }
    }
}