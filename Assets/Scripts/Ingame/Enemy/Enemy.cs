using System;
using System.Timers;
using Ingame.Player;
using Ingame.Player.Effect;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.Sound;

namespace Ingame
{
    public class Enemy : MonoBehaviour
    {
        private EnemyHitEffect _enemyHitEffect;

        private float _health;
        private float _damage = 1f;
        private float _speed = 1f;
        private float _speedMultiplier = 1;

        public float Damage => _damage;

        public event Action OnDie;
        private bool _isDie;

        public enum StatModifyType
        {
            SpeedMultiplier
        }

        private void Awake()
        {
            _enemyHitEffect = GetComponent<EnemyHitEffect>();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            var finalSpeed = _speed * _speedMultiplier;
            transform.position += transform.forward * Mathf.Max(finalSpeed * Time.deltaTime, 0);
        }

        private void OnDisable()
        {
            OnDie = null;
        }

        private void Init()
        {
            _isDie = false;
            _speedMultiplier = 1;
        }

        public void SetStats(float health, float damage, float speed)
        {
            _health = health;
            _damage = damage;
            _speed = speed;
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
            if (_isDie) return;

            if (!Mathf.Approximately(takenDamage, float.MaxValue))
            {
                FloatingTextManager.Instance.Display(transform.position + new Vector3(0, 2, 0), $"{takenDamage}");
                SoundManager.Instance.PlaySFX(SFXType.MonsterHit, 0.7f);
            }

            _health -= takenDamage;
            if (_enemyHitEffect) _enemyHitEffect.Play();
            if (_health <= 0) Die();
        }

        private void Die()
        {
            _isDie = true;
            OnDie?.Invoke();
            EffectManager.Instance.Clear(this);
        }
    }
}