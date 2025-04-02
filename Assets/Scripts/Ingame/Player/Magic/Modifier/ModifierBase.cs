using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Modifier
{
    public abstract class ModifierBase : MonoBehaviour
    {
        public abstract void Modify(Enemy enemy);
        public virtual void UnModify(Enemy enemy){}
    }
}