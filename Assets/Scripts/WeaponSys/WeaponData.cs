using UnityEngine;
using WeaponSys.WeaponTypes;

namespace WeaponSys
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [field: Header("General Weapon Stats")] 
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field:SerializeField] public string WeaponName { get; private set; }
        [field:SerializeField] public float FireRate { get; private set; }
        [field:SerializeField] public int MaxAmmo { get; private set; }

        [field:Header("Recoil and Spread")]
        [field:SerializeField] public float RecoilAmount { get; private set; }
        [field:SerializeField] public float BulletSpread { get; private set; }

       [field:Header("Shotgun Specific")]
       [field:SerializeField] public int PelletCount { get; private set; }
       [field:SerializeField] public float SpreadAngle { get; private set; }
    }
}