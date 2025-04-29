using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        public float AdditionalHealth;
        public float HealthMultiplier;
        public float AdditionalMana;
        public float ManaMultiplier;
        public float AdditionalManaRegen;
        public float ManaRegenMultiplier;
        public float AdditionalDamage;
        public float DamageMultiplier;

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
        
        public static PlayerStatsModifier operator +(PlayerStatsModifier left, PlayerStatsModifier right)
        {
            left.AdditionalHealth += right.AdditionalHealth;
            left.HealthMultiplier += right.HealthMultiplier;
            left.AdditionalMana += right.AdditionalMana;
            left.ManaMultiplier += right.ManaMultiplier;
            left.AdditionalManaRegen += right.AdditionalManaRegen;
            left.ManaRegenMultiplier += right.ManaRegenMultiplier;
            left.AdditionalDamage += right.AdditionalDamage;
            left.DamageMultiplier += right.DamageMultiplier;
            return left;
        }
    }

    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private PlayerFlatStats baseStats;
        private PlayerStatsModifier _consistentModifier = new PlayerStatsModifier(false);
        private PlayerFlatStats modifiedStats;

        private float _health;

        private float Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, modifiedStats.Mana);
                HUD.Instance.SetHealth(_health, modifiedStats.Health);
            }
        }

        private float _mana;

        private float Mana
        {
            get => _mana;
            set
            {
                _mana = Mathf.Clamp(value, 0, modifiedStats.Mana);
                HUD.Instance.SetMana(_mana, modifiedStats.Mana);
            }
        }

        //TODO 세이브 파일에서 로드
        public int CommandCount { get; private set; } = 1;

        private void Awake()
        {
            InitStats();
        }

        private void Update()
        {
            Mana += modifiedStats.ManaRegen * Time.deltaTime;
        }

        private void InitStats()
        {
            modifiedStats = _consistentModifier.Modify(baseStats);

            Health = modifiedStats.Health;
            Mana = modifiedStats.Mana;
        }

        private void ModifyStats(PlayerStatsModifier modifier)
        {
            _consistentModifier += modifier;
            modifiedStats = _consistentModifier.Modify(baseStats);
        }

        public bool UseMana(float mana)
        {
            if (mana > Mana)
            {
                //TODO: 마나 부족 메시지
                Debug.Log("마나 부족");
                return false;
            }

            Mana -= mana;
            return true;
        }
    }
}