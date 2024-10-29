using UnityEngine;

namespace WeaponSys.Bullets
{
    [CreateAssetMenu(fileName = "New bullet", menuName = "Weapons/Bullet Data", order = 0)]
    public class BulletData : ScriptableObject
    {
        [field:SerializeField] public float Damage {get; private set;}
        
        [field:SerializeField] public float InitialSpeed {get; private set;}
        
        [field:SerializeField] public float Lifetime {get; private set;}
        
    }
}