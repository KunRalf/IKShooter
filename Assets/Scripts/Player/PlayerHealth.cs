using System;
using Interfaces;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour, IDamageable, IRestorationArmor, IRestorationHealth
    {
        [SyncVar(hook = nameof(OnHealthChanged))]
        private float _currentHealth;

        [SyncVar(hook = nameof(OnArmorChanged))]
        private float _currentArmor;

        [SyncVar] private float _maxHealth;
        [SyncVar] private float _maxArmor;

        public float MaxHealth => _maxHealth;
        public float MaxArmor => _maxArmor;
        
        public event Action<float> OnHealthChangedEvent;
        public event Action<float> OnArmorChangedEvent;

     
        [Server]
        public void SetHeroStats(float health, float armor)
        {
            _maxHealth = health;
            _maxArmor = armor;
            _currentHealth = _maxHealth;
            _currentArmor = _maxArmor;
        }

        [Server]
        public void TakeDamage(float amount)
        {
            if (_currentHealth <= 0) return;

            float damageToHealth = amount;

            if (_currentArmor > 0)
            {
                float damageToArmor = Mathf.Min(_currentArmor, amount);
                _currentArmor -= damageToArmor;
                damageToHealth -= damageToArmor;
            }

            _currentHealth -= damageToHealth;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                RpcHandleDeath();
            }
        }

        private void OnHealthChanged(float o, float n)
        {
            OnHealthChangedEvent?.Invoke(n);
        }

        private void OnArmorChanged(float o, float n)
        {
            OnArmorChangedEvent?.Invoke(n);
        }

        [Server]
        public void RestoreArmor(float amount)
        {
            _currentArmor = Mathf.Min(_currentArmor + amount, _maxArmor);
        }

        [Server]
        public void RestoreHealth(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }

        [ClientRpc]
        private void RpcHandleDeath()
        {
            Debug.Log("Player is dead!");
        }
    }
}