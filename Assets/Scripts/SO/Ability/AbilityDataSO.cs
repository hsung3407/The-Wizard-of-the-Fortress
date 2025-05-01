using System.Collections.Generic;
using Ingame.Player;
using UnityEngine;

namespace SO.Ability
{
    public abstract class AbilityDataSO : ScriptableObject
    {
        public enum AbilityRarity
        {
            Common,
            Uncommon,
            Rare,
            Unique,
            Epic,
            Legendary,
        }

        [field: SerializeField] public string AbilityName { get; private set; }
        [field: SerializeField] public List<string> Tags { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string AbilityDescription { get; private set; }
        [field: SerializeField] public AbilityRarity Rarity { get; private set; }

        public abstract Ingame.Player.Ability.Ability CreateAbility(Player player);
    }
}