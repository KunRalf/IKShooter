using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnHealthChanged))]
        private int currentHealth;

        [SyncVar(hook = nameof(OnArmorChanged))]
        private int currentArmor;

        [SyncVar] private int maxHealth;
        [SyncVar] private int maxArmor;

        public delegate void HealthChanged(int newHealth);
        public event HealthChanged OnHealthChangedEvent;
        public delegate void ArmorChanged(int newArmor);
        public event ArmorChanged OnArmorChangedEvent;

        public override void OnStartServer()
        {
       
        }

        [Server]
        public void SetHeroStats(int health, int armor)
        {
            maxHealth = health;
            maxArmor = armor;
            currentHealth = maxHealth;
            currentArmor = maxArmor;
        }

        [Server]
        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0) return;

            int damageToHealth = amount;

            if (currentArmor > 0)
            {
                int damageToArmor = Mathf.Min(currentArmor, amount);
                currentArmor -= damageToArmor;
                damageToHealth -= damageToArmor;
            }

            currentHealth -= damageToHealth;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                RpcHandleDeath();
            }
        }

        private void OnHealthChanged(int oldHealth, int newHealth)
        {
            OnHealthChangedEvent?.Invoke(newHealth);
        }

        private void OnArmorChanged(int oldArmor, int newArmor)
        {
            OnArmorChangedEvent?.Invoke(newArmor);
        }

        [Server]
        public void HealArmor(int amount)
        {
            currentArmor = Mathf.Min(currentArmor + amount, maxArmor);
        }

        [Server]
        public void HealHealth(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }

        [ClientRpc]
        private void RpcHandleDeath()
        {
            Debug.Log("Player is dead!");
        }
    }
}