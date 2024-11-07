using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerUI : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnHealthChanged))] private float _heathBarRatio;
        [SyncVar(hook = nameof(OnArmorChanged))] private float _armorBarRatio;
        [SerializeField] private Image _heathBar;
        [SerializeField] private Image _armorBar;
        [SerializeField] private Gradient _healthBarHealthGradient;

        [SyncVar] private float _maxHealth;
        [SyncVar] private float _maxArmor;

        [Server]
        public void UpdateHealthBar(float value)
        {
            _heathBarRatio = value;
        }  
        
        [Server]
        public void UpdateArmorBar(float value)
        {
            _armorBarRatio = value;
        }
        
        private void OnHealthChanged(float o, float n)
        {
             float fill = n / _maxHealth;
            _heathBar.fillAmount = fill;
            _heathBar.color = _healthBarHealthGradient.Evaluate(fill);
        }

        private void OnArmorChanged(float o, float n)
        {
            float fill = n / _maxArmor;
            _armorBar.fillAmount = fill;
        }
    }
}