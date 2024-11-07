using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Players/PlayerStats")]
    public class PlayerTypeStats : ScriptableObject
    {
        [field:SerializeField] public float MaxHealth { get; set; }
        [field:SerializeField] public float MaxArmor { get; set; }
        [field:SerializeField] public float MoveSpeed { get; set; }
    }
}