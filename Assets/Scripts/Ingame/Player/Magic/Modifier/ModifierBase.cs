using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Modifier
{
    public abstract class ModifierBase : MonoBehaviour
    {
        public abstract void Modify(Mob mob);
        public abstract void UnModify(Mob mob);
    }

    [Serializable]
    public class DebuffInfo
    {
        [SerializeField] private Mob.StatType statType;
        [SerializeField] private float amount;
        
        public Mob.StatType StatType => statType;
        public float Amount => amount;
    }
}