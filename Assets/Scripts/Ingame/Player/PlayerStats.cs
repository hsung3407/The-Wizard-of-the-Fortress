using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility.Sound;

namespace Ingame.Player
{
    [Serializable]
    public class PlayerFlatStats
    {
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Mana { get; private set; }
        [field: SerializeField] public float ManaRegen { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }

        public PlayerFlatStats(float health, float mana, float manaRegen, float damage)
        {
            Health = health;
            Mana = mana;
            ManaRegen = manaRegen;
            Damage = damage;
        }
    }
    
    public class PlayerStatsModifier
    {
        // A = additional, M = Multiplier
        public enum PlayerStatsType
        {
            AHealth,
            MHealth,
            AMana,
            MMana,
            AManaRegen,
            MManaRegen,
            ADamage,
            MDamage
        }

        public float AdditionalHealth { get; private set; }
        public float HealthMultiplier { get; private set; }
        public float AdditionalMana { get; private set; }
        public float ManaMultiplier { get; private set; }
        public float AdditionalManaRegen { get; private set; }
        public float ManaRegenMultiplier { get; private set; }
        public float AdditionalDamage { get; private set; }
        public float DamageMultiplier { get; private set; }

        public PlayerStatsModifier(bool isEmpty = true)
        {
            AdditionalHealth = 0;
            HealthMultiplier = isEmpty ? 0 : 1;
            AdditionalMana = 0;
            ManaMultiplier = isEmpty ? 0 : 1;
            AdditionalManaRegen = 0;
            ManaRegenMultiplier = isEmpty ? 0 : 1;
            AdditionalDamage = 0;
            DamageMultiplier = isEmpty ? 0 : 1;
        }

        public PlayerFlatStats Modify(PlayerFlatStats original)
        {
            return new PlayerFlatStats(
                (original.Health + AdditionalHealth) * HealthMultiplier,
                (original.Mana + AdditionalMana) * ManaMultiplier,
                (original.ManaRegen + AdditionalManaRegen) * ManaRegenMultiplier,
                (original.Damage + AdditionalDamage) * DamageMultiplier);
        }

        public void Add(PlayerStatsType type, float amount)
        {
            switch (type)
            {
                case PlayerStatsType.AHealth:
                    AdditionalHealth += amount;
                    break;
                case PlayerStatsType.MHealth:
                    HealthMultiplier += amount;
                    break;
                case PlayerStatsType.AMana:
                    AdditionalMana += amount;
                    break;
                case PlayerStatsType.MMana:
                    ManaMultiplier += amount;
                    break;
                case PlayerStatsType.AManaRegen:
                    AdditionalManaRegen += amount;
                    break;
                case PlayerStatsType.MManaRegen:
                    ManaRegenMultiplier += amount;
                    break;
                case PlayerStatsType.ADamage:
                    AdditionalDamage += amount;
                    break;
                case PlayerStatsType.MDamage:
                    DamageMultiplier += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private PlayerFlatStats baseStats;
        private PlayerStatsModifier _consistentModifier = new PlayerStatsModifier(false);
        public PlayerFlatStats ModifiedStats { get; private set; }

        private float _health;

        private float Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, ModifiedStats.Health);
                HUD.Instance.SetHealth(_health, ModifiedStats.Health);
                if(_health <= 0) Die();
            }
        }

        private float _mana;

        private float Mana
        {
            get => _mana;
            set
            {
                _mana = Mathf.Clamp(value, 0, ModifiedStats.Mana);
                HUD.Instance.SetMana(_mana, ModifiedStats.Mana);
            }
        }

        public event Action OnDie;

        //TODO 세이브 파일에서 로드
        public int CommandCount { get; private set; } = 1;

        private void Awake()
        {
            InitStats();
        }

        private void Update()
        {
            Mana += ModifiedStats.ManaRegen * Time.deltaTime;
        }

        private void InitStats()
        {
            StatsUpdate();

            Health = ModifiedStats.Health;
            Mana = ModifiedStats.Mana;
        }

        private void StatsUpdate()
        {
            ModifiedStats = _consistentModifier.Modify(baseStats);

            HUD.Instance.SetHealth(_health, ModifiedStats.Health);
            HUD.Instance.SetMana(_mana, ModifiedStats.Mana);
        }

        public void ModifyStats(PlayerStatsModifier.PlayerStatsType type, float amount)
        {
            _consistentModifier.Add(type, amount);
            StatsUpdate();
        }

        public bool UseMana(float mana)
        {
            if (mana > Mana)
            {
                NotificationManager.Instance.NotifyError("Lack - Mana");
                return false;
            }

            Mana -= mana;
            return true;
        }
        
        public void TakeHit(float damage)
        {
            Health -= damage;
        }

        private void Die()
        {
            Debug.Log("Die");
            OnDie?.Invoke();
            SoundManager.Instance.PlaySFX(SFXType.WallBreak, 0.3f);
        }
    }
}