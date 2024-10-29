using System;
using UnityEngine;

namespace WeaponSys.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletData _bulletData; // ScriptableObject с параметрами пули
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.AddForce(transform.forward * _bulletData.InitialSpeed, ForceMode.Impulse);
            Destroy(gameObject, _bulletData.Lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            // Enemy enemy = collision.transform.GetComponent<Enemy>();
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(_bulletData.damage);
            // }

            Debug.Log("Ударилось");
            Destroy(gameObject);
        }
    }
}