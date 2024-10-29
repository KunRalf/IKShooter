using UnityEngine;

namespace WeaponSys.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletData _bulletData; // ScriptableObject с параметрами пули
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            // Применяем силу к Rigidbody для начального ускорения пули
            _rb.AddForce(transform.forward * _bulletData.InitialSpeed, ForceMode.Impulse);

            // Уничтожаем пулю через заданное время жизни, если она не столкнётся с объектом
            Destroy(gameObject, _bulletData.Lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            // Enemy enemy = collision.transform.GetComponent<Enemy>();
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(_bulletData.damage);
            // }

          
            Destroy(gameObject);
        }
    }
}