using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float _health;
        
        [SerializeField] private float damage = 1f;
        public float Damage => damage;

        [SerializeField] private float speed = 1;
        private float _speedMultiplier = 1;

        private event Action OnDie;
        
        public enum StatType
        {
            Speed,
            SpeedMultiplier
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            transform.position += transform.forward * Mathf.Max(speed * _speedMultiplier * Time.deltaTime, 0);
        }

        private void Init()
        {
            _health = maxHealth;
        }

        public void Init(Action onDie)
        {
            OnDie = onDie;
            Init();
        }

        public void ModifyStat(StatType statType, float amount)
        {
            switch (statType)
            {
                case StatType.Speed:
                    speed += amount;
                    break;
                case StatType.SpeedMultiplier:
                    _speedMultiplier += amount;
                    break;
                default:
                    break;
            }
        }

        public void TakeDamage(float cursedDamage)
        {
            _health -= cursedDamage;
            if(_health <= 0) Die();
        }

        private void Die()
        {
            OnDie?.Invoke();
        }
    }
}
