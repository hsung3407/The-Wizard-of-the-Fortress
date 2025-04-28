using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ingame.Player.Effect.Command
{
    [Serializable]
    public abstract class TimeLimitEffectData
    {
        [field: SerializeField] public float Duration { get; private set; }

        public abstract TimeLimitEffectCommand GetCommand(EffectID effectID, Enemy enemy);
    }

    public abstract class TimeLimitEffectCommand : EffectCommand
    {
        public float StartTime { get; private set; }
        public float Duration { get; private set; }
        public float EndTime { get; private set; }

        public TimeLimitEffectCommand(EffectID effectID, Object target, float duration) : base(effectID, target)
        {
            StartTime = Time.time;
            Duration = duration;
            EndTime = StartTime + Duration;
        }
        
        public override bool IsExpired()
        {
            return EndTime <= Time.time;
        }
    }
    
    [Serializable]
    public abstract class TimeLimitEnemyEffectData : TimeLimitEffectData
    {
        [field: SerializeField] public Enemy.StatModifyType StatModifyType { get; private set; }
        [field: SerializeField] public float Amount { get; private set; }
        
        public override TimeLimitEffectCommand GetCommand(EffectID effectID, Enemy enemy)
        {
            return new TimeLimitEnemyEffectCommand(effectID, enemy, Duration, StatModifyType, Amount);
        }
    }

    public class TimeLimitEnemyEffectCommand : TimeLimitEffectCommand
    {
        private readonly Enemy _enemy;
        private readonly Enemy.StatModifyType _statModifyType;
        private readonly float _amount;
        
        public TimeLimitEnemyEffectCommand(EffectID effectID, Enemy enemy, float duration, Enemy.StatModifyType statModifyType, float amount) : base(effectID, enemy, duration)
        {
            _enemy = enemy;
            _statModifyType = statModifyType;
            _amount = amount;
        }
        
        public override void Execute()
        {
            _enemy.ModifyStat(_statModifyType, _amount);
        }

        public override void Release()
        {
            _enemy.ModifyStat(_statModifyType, -_amount);
        }
    }
}