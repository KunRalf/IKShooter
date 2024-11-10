using System;
using Mirror;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealthBar : NetworkBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private GameObject _barPanel;
        [SyncVar(hook = nameof(OnHealthChanged))] private float _heathBarRatio;
        [SyncVar(hook = nameof(OnArmorChanged))] private float _armorBarRatio;
        [SerializeField] private Image _heathBar;
        [SerializeField] private TextMeshProUGUI _hpT;
        [SerializeField] private TextMeshProUGUI _arT;
        [SerializeField] private Image _armorBar;
        [SerializeField] private Gradient _healthBarHealthGradient;
        private Camera _camLook;
        
        public override void OnStartClient()
        {
            _playerHealth.OnHealthChangedEvent += UpdateHealthBar;
            _playerHealth.OnArmorChangedEvent += UpdateArmorBar;
            _barPanel.SetActive(!isOwned);
        }
        
        

        private void Update()
        {
            if (!isOwned)
            {
                _barPanel.transform.rotation = Quaternion.LookRotation(transform.position - _camLook.transform.position);
            }
        }

        private void Start()
        {
            _camLook = Camera.main;
        }

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
             float fill = n / _playerHealth.MaxHealth;
            _heathBar.fillAmount = fill;
            _heathBar.color = _healthBarHealthGradient.Evaluate(fill);
            _hpT.text = n.ToString();
        }

        private void OnArmorChanged(float o, float n)
        {
            float fill = n / _playerHealth.MaxArmor;
            _armorBar.fillAmount = fill;
            _arT.text = n.ToString();
        }
    }
}