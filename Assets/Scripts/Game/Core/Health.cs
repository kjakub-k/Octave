using System;
using UnityEngine;
namespace KJakub.Octave.Game.Core
{
    public class Health
    {
        public int Amount { get; private set; }
        public int MaxHealth { get; private set; }
        public event Action<int> OnHealthAdded;
        public event Action<int> OnHealthRemoved;
        public event Action<int> OnDeath;
        public Health(int maxHealth)
        {
            MaxHealth = maxHealth;
            Amount = maxHealth;
        }
        public void Heal(int amount)
        {
            Amount += amount;

            OnHealthAdded?.Invoke(Amount);

            if (Amount > MaxHealth)
            {
                Amount = MaxHealth;
            }
        }
        public void Damage(int amount)
        {
            Amount -= amount;

            OnHealthRemoved?.Invoke(Amount);

            if (Amount <= 0)
            {
                Amount = 0;
                OnDeath?.Invoke(Amount);
            }
        }
    }
}