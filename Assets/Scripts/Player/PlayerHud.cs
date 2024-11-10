using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _armor;
        [SerializeField] private TextMeshProUGUI _ammo;

        public void UpdateHealth(float health)
        {
            _health.text = ((int)health).ToString();
        }
        
        public void UpdateArmor(float armor)
        {
            _armor.text = ((int)armor).ToString();
        } 
        
        public void UpdateAmmo(int ammo)
        {
            _ammo.text = ammo.ToString();
        }
    }
}