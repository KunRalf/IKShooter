using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSys;

namespace Player
{
    public class PlayerWeaponSystem : NetworkBehaviour
    {
        // [SerializeField] private List<Weapon> _weapons;
        [SerializeField] public RaycastWeapon _weapon;
        [SerializeField] private PlayerSetRig _playerSetRig;
        
        private int currentWeaponIndex = 0;

        private void Start()
        {
            // EquipWeapon(currentWeaponIndex);
        }

        private void Update()
        {
            
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CmdShoot();
                }
                if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    _weapon.StopFireing();
                }
                if(_weapon.IsFireing)
                {
                    _weapon.UpdateFireing(Time.deltaTime);
                }
                _weapon.UpdateBullets(Time.deltaTime);
            
            
            
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     currentWeaponIndex = (currentWeaponIndex + 1) % _weapons.Count;
            //     EquipWeapon(currentWeaponIndex);
            // }
            // else if (Input.GetKeyDown(KeyCode.E))
            // {
            //     currentWeaponIndex = (currentWeaponIndex - 1 + _weapons.Count) % _weapons.Count;
            //     EquipWeapon(currentWeaponIndex);
            // }
            //
            // if (Input.GetKey(KeyCode.Mouse0))
            // {
            //     CmdShoot();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     _weapons[currentWeaponIndex].Reload();
            // }
        }

        [Command]
        private void CmdShoot()
        {
            _weapon.StartFireing();
        }
        
        // private void EquipWeapon(int index)
        // {
        //     for (int i = 0; i < _weapons.Count; i++)
        //     {
        //         _weapons[i].gameObject.SetActive(i == index);
        //         if(i == index)
        //             _playerSetRig.SetHandsOnWeapon(_weapons[i].LHand,_weapons[i].RHand);
        //     }
        // }
    }
}