using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSys;

namespace Player
{
    public class PlayerWeaponSystem : MonoBehaviour
    {
        [SerializeField] private List<Weapon> _weapons;
        [SerializeField] private PlayerSetRig _playerSetRig;
        
        private int currentWeaponIndex = 0;

        private void Start()
        {
            EquipWeapon(currentWeaponIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % _weapons.Count;
                EquipWeapon(currentWeaponIndex);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                currentWeaponIndex = (currentWeaponIndex - 1 + _weapons.Count) % _weapons.Count;
                EquipWeapon(currentWeaponIndex);
            }

            if (Input.GetButton("Fire1"))
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
                if(i == index)
                    _playerSetRig.SetHandsOnWeapon(_weapons[i].LHand,_weapons[i].RHand);
            }
        }
    }
}