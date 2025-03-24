using System.Collections.Generic;
using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
    public class Debuffer : ModifierBase
    {
        [SerializeField] private List<DebuffInfo> debuffs = new List<DebuffInfo>();
        
        public override void Modify(Mob mob)
        {
            foreach (var debuffInfo in debuffs)
            {
                mob.ModifyStat(debuffInfo.StatType, debuffInfo.Amount);  
            }
        }

        public override void UnModify(Mob mob)
        {
            foreach (var debuffInfo in debuffs)
            {
                mob.ModifyStat(debuffInfo.StatType, -debuffInfo.Amount);  
            }
        }
    }
}