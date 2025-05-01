using System.Collections.Generic;
using SO;
using SO.Ability;
using UnityEngine;

namespace Ingame.Player.Ability
{
    public abstract class Ability
    {
        public readonly Player Player;
        public readonly AbilityDataSO Data;
        //0 is infinite
        public readonly int MaxLevel;
        public int Level { get; private set; }

        protected Ability(Player player, AbilityDataSO data, int maxLevel = 1)
        {
            Player = player;
            Data = data;
            MaxLevel = maxLevel;
            Level = 0;
        }

        public virtual void OnAdded()
        {
            Upgrade();
            Debug.Log($"Added Ability : {Data.AbilityName}");
        }

        public virtual void OnRemoved()
        {
            Debug.Log($"Removed Ability : {Data.AbilityName}");
        }

        public bool IsSelectable()
        {
            return Level < MaxLevel || MaxLevel == 0;
        }

        public void Upgrade()
        {
            Level++;
            OnUpgraded();
        }

        protected abstract void OnUpgraded();
    }
}