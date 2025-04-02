using System;
using System.Collections.Generic;
using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
    [Serializable]
    public class DebuffInfo
    {
        [SerializeField] private Enemy.StatType statType;
        [SerializeField] private float amount;
        
        public Enemy.StatType StatType => statType;
        public float Amount => amount;
    }
    
    public class Debuffer : ModifierBase
    {
        [SerializeField] private List<DebuffInfo> debuffs = new List<DebuffInfo>();
        
        public override void Modify(Enemy enemy)
        {
            foreach (var debuffInfo in debuffs)
            {
                enemy.ModifyStat(debuffInfo.StatType, debuffInfo.Amount);  
            }
        }

        public override void UnModify(Enemy enemy)
        {
            foreach (var debuffInfo in debuffs)
            {
                enemy.ModifyStat(debuffInfo.StatType, -debuffInfo.Amount);  
            }
        }
    }
}