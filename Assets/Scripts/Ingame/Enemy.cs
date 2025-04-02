using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    [Serializable]
    public class EnemyBaseStat
    {
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float damage = 1;
        [SerializeField] private float speed = 1;
        [SerializeField] private float speedMultiplier = 1;
        
        public float MaxHealth => maxHealth;
        public float Damage => damage;
        public float Speed => speed;
        public float SpeedMultiplier => speedMultiplier;
        
    }
    
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyBaseStat baseStat;
        
        private float _health;
        private float _damage = 1f;
        private float _speed = 1;
        private float _speedMultiplier = 1;

        public float Damage => _damage;
        
        private event Action OnDie;
        private bool isDie;

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
            transform.position += transform.forward * Mathf.Max(_speed * _speedMultiplier * Time.deltaTime, 0);
        }

        private void Init()
        {
            isDie = false;
            _health = baseStat.MaxHealth;
            _damage = baseStat.Damage;
            _speed = baseStat.Speed;
            _speedMultiplier = baseStat.SpeedMultiplier;
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
                    _speed += amount;
                    break;
                case StatType.SpeedMultiplier:
                    _speedMultiplier += amount;
                    break;
                default:
                    break;
            }
        }

        public void TakeDamage(float takenDamage)
        {
            _health -= takenDamage;
            if (_health <= 0 && !isDie) Die();
        }

        private void Die()
        {
            isDie = true;
            OnDie?.Invoke();
        }
    }
}