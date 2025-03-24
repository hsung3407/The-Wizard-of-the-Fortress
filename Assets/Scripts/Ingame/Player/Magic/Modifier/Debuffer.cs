using System.Collections.Generic;
using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
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