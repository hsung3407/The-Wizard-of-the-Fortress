using System;
using System.Collections.Generic;
using Ingame.Player.Modifier;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Modifier
{
    public class SimpleTimeLimitEnemyEffectCommand : EffectCommand
    {
        private readonly Enemy _enemy;
        private readonly Enemy.StatModifyType _statModifyType;
        private readonly float _amount;
        public float StartTime { get; private set; }
        public float LastDuration { get; private set; }
        public float EndTime { get; private set; }

        public SimpleTimeLimitEnemyEffectCommand(SimpleEffectData data, Enemy enemy, int ownerID) : base(new EffectID(data.EffectID, ownerID), enemy)
        {
            _enemy = enemy;
            _statModifyType = data.StatModifyType;
            _amount = data.Amount;
            StartTime = Time.time;
            LastDuration = data.Duration;
            EndTime = StartTime + LastDuration;
        }

        public override void Execute()
        {
            _enemy.ModifyStat(_statModifyType, _amount);
        }

        public override void Release()
        {
            _enemy.ModifyStat(_statModifyType, -_amount);
        }

        public override bool IsExpired()
        {
            return EndTime <= Time.time;
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
            Debug.Log(effectData.EffectID.GetHashCode());
            EffectManager.Instance.Add(new SimpleTimeLimitEnemyEffectCommand(effectData, enemy, GetInstanceID()));
        }

        public override void UnModify([NotNull] Enemy enemy)
        {
            EffectManager.Instance.Remove(enemy, effectData.EffectID);
        }
    }
}