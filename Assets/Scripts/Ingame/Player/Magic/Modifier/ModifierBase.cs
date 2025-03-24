using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Modifier
{
    public abstract class ModifierBase : MonoBehaviour
    {
        public abstract void Modify(Enemy enemy);
        public abstract void UnModify(Enemy enemy);
    }

    [Serializable]
    public class DebuffInfo
    {
        [SerializeField] private Enemy.StatType statType;
        [SerializeField] private float amount;
        
        public Enemy.StatType StatType => statType;
        public float Amount => amount;
    }
}