using System;
using System.Collections.Generic;
using Ingame.Player.Modifier;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Modifier
{
    public class SimpleEffectCommand : EffectCommand
    {
        private readonly Enemy.StatModifyType _statModifyType;
        private readonly float _amount;

        public SimpleEffectCommand(SimpleEffectData data, Enemy enemy) : base(data.EffectID, enemy, data.Duration)
        {
            _statModifyType = data.StatModifyType;
            _amount = data.Amount;
        }

        public override void Execute()
        {
            Enemy.ModifyStat(_statModifyType, _amount);
        }

        public override void Release()
        {
            Enemy.ModifyStat(_statModifyType, -_amount);
        }
    }

    [Serializable]
    public class SimpleEffectData
    {
        [field: SerializeField] public EffectID EffectID { get; private set; }
        [field: SerializeField] public Enemy.StatModifyType StatModifyType { get; private set; }
        [field: SerializeField] public float Amount { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }

    public class Effector : ModifierBase
    {
        [SerializeField] private SimpleEffectData effectData;

        public override void Modify([NotNull] Enemy enemy)
        {
            EffectManager.Instance.Add(new SimpleEffectCommand(effectData, enemy));
        }

        public override void UnModify([NotNull] Enemy enemy)
        {
            EffectManager.Instance.Remove(enemy, effectData.EffectID);
        }
    }
}