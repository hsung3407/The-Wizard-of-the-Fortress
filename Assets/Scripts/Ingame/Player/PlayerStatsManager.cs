using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ingame.Player
{
    [Serializable]
    public class PlayerStats
    {
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Mana { get; private set; }
        [field: SerializeField] public float ManaRegen { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }

        public PlayerStats(float health, float mana, float manaRegen, float damage)
        {
            Health = health;
            Mana = mana;
            ManaRegen = manaRegen;
            Damage = damage;
        }
    }

    public class PlayerStatsModifier
    {
        public float AdditionalHealth = 0;
        public float HealthMultiplier = 1;
        public float AdditionalMana = 0;
        public float ManaMultiplier = 1;
        public float AdditionalManaRegen = 0;
        public float ManaRegenMultiplier = 1;
        public float AdditionalDamage = 0;
        public float DamageMultiplier = 1;

        public PlayerStats Modify(PlayerStats original)
        {
            return new PlayerStats(
                (original.Health + AdditionalHealth) * HealthMultiplier,
                (original.Mana + AdditionalMana) * ManaMultiplier,
                (original.ManaRegen + AdditionalManaRegen) * ManaRegenMultiplier,
                (original.Damage + AdditionalDamage) * DamageMultiplier);
        }
    }

    public class PlayerStatsManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats baseStats;
        private PlayerStatsModifier _consistentModifier = new PlayerStatsModifier();
        private PlayerStats _modifiedStats;

        private float _health;

        private float Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, _modifiedStats.Mana);
                HUD.Instance.SetHealth(_health, _modifiedStats.Health);
            }
        }

        private float _mana;

        private float Mana
        {
            get => _mana;
            set
            {
                _mana = Mathf.Clamp(value, 0, _modifiedStats.Mana);
                HUD.Instance.SetMana(_mana, _modifiedStats.Mana);
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
            Mana += _modifiedStats.ManaRegen * Time.deltaTime;
        }

        private void InitStats()
        {
            _modifiedStats = _consistentModifier.Modify(baseStats);

            Health = _modifiedStats.Health;
            Mana = _modifiedStats.Mana;
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