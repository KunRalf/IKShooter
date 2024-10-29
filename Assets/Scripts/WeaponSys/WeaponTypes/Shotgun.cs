using WeaponSys.Bullets;

namespace WeaponSys.WeaponTypes
{
    public class Shotgun : Weapon
    {
        public override void Shoot()
        {
            if (!CanShoot()) return;
            
       
            for (int i = 0; i < _weaponData.PelletCount; i++)
            {
                Bullet bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            }

            _currentAmmo--;
            UpdateFireCooldown();
        }
        
    
    }
}