using System.Collections.Generic;
using Ingame.Player;
using Ingame.Player.Ability;
using UnityEngine;

namespace SO.Ability
{
    [CreateAssetMenu(fileName = "ConsistentAbilityDataSO", menuName = "Scriptable Objects/AbilityDataSO/ConsistentAbilityDataSO")]
    public class ConsistentAbilityDataSO : AbilityDataSO
    {
        public enum LevelUpgradeType
        {
            Add,
            Change,
            Repeat
        }
        
        [field: SerializeField] public PlayerStatsModifier.PlayerStatsType StatsType {get; private set;}
        [field: SerializeField] public LevelUpgradeType UpgradeType {get; private set;}
        [field: SerializeField] public List<float> Amounts {get; private set;}
        public override Ingame.Player.Ability.Ability CreateAbility(Player player)
        {
            return new ConsistentAbility(player, this);
        }
    }
}