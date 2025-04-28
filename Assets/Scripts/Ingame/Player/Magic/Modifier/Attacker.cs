using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
    public class Attacker : ModifierBase
    {
        private float _damage;

        public override void Init(MagicDataSO magicData, MagicStats modifiedStats)
        {
            _damage = modifiedStats.Damage;
        }

        public override void Modify(Enemy enemy)
        {
            enemy.TakeDamage(_damage);
        }
    }
}