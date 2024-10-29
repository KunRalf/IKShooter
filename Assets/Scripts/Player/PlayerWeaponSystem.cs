using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSys;

namespace Player
{
    public class PlayerWeaponSystem : MonoBehaviour
    {
        [SerializeField] private List<Weapon> _weapons;   // Массив всех видов оружия
        private int currentWeaponIndex = 0;

        private void Start()
        {
            EquipWeapon(currentWeaponIndex);
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % _weapons.Count;
                EquipWeapon(currentWeaponIndex);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                currentWeaponIndex = (currentWeaponIndex - 1 + _weapons.Count) % _weapons.Count;
                EquipWeapon(currentWeaponIndex);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                _weapons[currentWeaponIndex].Shoot();
            }

            // if (Input.GetButtonDown("Reload"))
            // {
            //     _weapons[currentWeaponIndex].Reload();
            // }
        }

        private void EquipWeapon(int index)
        {
            for (int i = 0; i < _weapons.Count; i++)
            {
                _weapons[i].gameObject.SetActive(i == index);
            }
        }
    }
}