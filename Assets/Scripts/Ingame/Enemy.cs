using System;
using System.Timers;
using Ingame.Player;
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

        public float MaxHealth => maxHealth;
        public float Damage => damage;
        public float Speed => speed;
    }

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyBaseStat baseStat;

        private float _health;
        private float _damage = 1f;
        private float _speedMultiplier = 1;

        public float Damage => _damage;

        public event Action OnDie;
        private bool _isDie;

        public enum StatModifyType
        {
            SpeedMultiplier
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            var finalSpeed = baseStat.Speed * _speedMultiplier;
            transform.position += transform.forward * Mathf.Max(finalSpeed * Time.deltaTime, 0);
        }

        private void OnDisable()
        {
            OnDie = null;
        }

        private void Init()
        {
            _isDie = false;
            _health = baseStat.MaxHealth;
            _damage = baseStat.Damage;
            _speedMultiplier = 1;
        }

        public void ModifyStat(StatModifyType statModifyType, float amount)
        {
            switch (statModifyType)
            {
                case StatModifyType.SpeedMultiplier:
                    _speedMultiplier += amount;
                    break;
                default:
                    break;
            }
        }

        public void TakeDamage(float takenDamage)
        {
            _health -= takenDamage;
            if (_health <= 0 && !_isDie) Die();
        }

        private void Die()
        {
            _isDie = true;
            EffectManager.Instance.Clear(this);
            OnDie?.Invoke();
        }
    }
}