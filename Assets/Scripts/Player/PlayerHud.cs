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
        [SerializeField] private TextMeshProUGUI _interactText;

        public void EnableInteractText(string value)
        {
            _interactText.text = value;
            _interactText.gameObject.SetActive(true);
        }

        public void DisableInteractText()
        {
            _interactText.gameObject.SetActive(true);
            _interactText.text = string.Empty;
        }
        
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