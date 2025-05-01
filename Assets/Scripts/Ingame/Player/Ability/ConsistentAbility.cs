using System;
using SO;
using SO.Ability;
using UnityEngine;

namespace Ingame.Player.Ability
{
    public class ConsistentAbility : Ability
    {
        public readonly ConsistentAbilityDataSO ConsistentData;
        
        public ConsistentAbility(Player player, ConsistentAbilityDataSO data) : base(player, data)
        {
            ConsistentData = data;
            int maxLevel = data.UpgradeType == ConsistentAbilityDataSO.LevelUpgradeType.Repeat ? 0 : data.Amounts.Count;
        }
        
        protected override void OnUpgraded()
        {
            Player.PlayerStats.ModifyStats(ConsistentData.StatsType, GetAmount());
        }

        public float GetAmount()
        {
            return ConsistentData.UpgradeType switch
            {
                ConsistentAbilityDataSO.LevelUpgradeType.Add => ConsistentData.Amounts[Level - 1],
                ConsistentAbilityDataSO.LevelUpgradeType.Change => ConsistentData.Amounts[Level - 1]
                                                                   - (Level == 1
                                                                       ? 0
                                                                       : ConsistentData.Amounts[Level - 2]),
                ConsistentAbilityDataSO.LevelUpgradeType.Repeat => ConsistentData.Amounts[
                    Mathf.Max(Level - 1, ConsistentData.Amounts.Count - 1)],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}