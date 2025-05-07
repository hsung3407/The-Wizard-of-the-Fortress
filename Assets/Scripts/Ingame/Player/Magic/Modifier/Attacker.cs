using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
    public class Attacker : ModifierBase
    {
        private float _damage;

        public override void Init(MagicDataSO magicData, MagicStats modifiedMagicStats, PlayerFlatStats playerStats)
        {
            _damage = modifiedMagicStats.GetDamage(playerStats);
        }

        public override void Modify(Enemy enemy)
        {
            enemy.TakeDamage(_damage);
        }
    }
}