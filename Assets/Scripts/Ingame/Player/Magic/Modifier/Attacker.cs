using Ingame.Player.Modifier;
using UnityEngine;

namespace Ingame.Player.Magic.Modifier
{
    public class Attacker : ModifierBase
    {
        [SerializeField] private float damage;
        
        public override void Modify(Enemy enemy)
        {
            enemy.TakeDamage(damage);
        }
    }
}