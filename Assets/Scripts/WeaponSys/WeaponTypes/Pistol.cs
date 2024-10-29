using WeaponSys.Bullets;

namespace WeaponSys.WeaponTypes
{
    public class Pistol : Weapon
    {
        public override void Shoot()
        {
            if (!CanShoot()) return;

            Bullet bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);

            _currentAmmo--;
            UpdateFireCooldown();
        }
    }
}