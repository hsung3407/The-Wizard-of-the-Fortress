using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Utility.SingleTon;

namespace Ingame.Player
{
    [Serializable]
    public class EffectID
    {
        [SerializeField] private string effectGroupName;
        [SerializeField] private string effectName;

        [field: SerializeField] public CompareType ObjectCompareType { get; set; }
        [field: SerializeField] public CompareType EffectCompareType { get; private set; }
        
        public EffectID(string effectGroupName, string effectName)
        {
            this.effectGroupName = effectGroupName;
            this.effectName = effectName;
        }

        public enum CompareType
        {
            All,
            WithoutHash,
            GroupName,
            EffectName,
        }

        public bool Compare(EffectID other)
        {
            return Compare(this, other, other.ObjectCompareType);
        }

        public bool Compare(EffectID other, CompareType compareType)
        {
            return Compare(this, other, compareType);
        }

        public static bool Compare(EffectID a, EffectID b, CompareType compareType)
        {
            bool checkGroup = a?.effectGroupName == b?.effectGroupName;
            bool checkName = a?.effectName == b?.effectName;
            bool checkHash = a?.GetHashCode() == b?.GetHashCode();
            return compareType switch
            {
                CompareType.All => checkGroup && checkName && checkHash,
                CompareType.WithoutHash => checkGroup && checkName,
                CompareType.GroupName => checkGroup,
                CompareType.EffectName => checkName,
                _ => throw new ArgumentOutOfRangeException(nameof(compareType), compareType, null)
            };
        }

        public override string ToString()
        {
            return $"[{effectGroupName}]{effectName}";
        }
    }

    public abstract class EffectCommand
    {
        public readonly EffectID EffectID;
        public readonly Enemy Enemy;
        public readonly float StartTime;
        public float LastDuration { get; private set; }
        public float EndTime { get; private set; }

        protected EffectCommand(EffectID effectID, Enemy enemy, float lastDuration)
        {
            EffectID = effectID;
            Enemy = enemy;
            StartTime = Time.time;
            LastDuration = lastDuration;
            EndTime = StartTime + LastDuration;
        }

        public abstract void Execute();
        public abstract void Release();

        public void ResetTime(float duration)
        {
            LastDuration = duration;
            EndTime = Time.time + duration;
        }

        public void ExtendTime(float duration)
        {
            var newEndTime = Time.time + duration;
            if (newEndTime < EndTime) { return; }

            LastDuration = duration;
            EndTime = newEndTime;
        }

        public void AddTime(float duration)
        {
            LastDuration = duration;
            EndTime += duration;
        }
    }

    public class EffectCommandList : LinkedList<EffectCommand>
    {
        public void Add(EffectCommand effectCommand)
        {
            if (!Contains(effectCommand.EffectID, effectCommand.EffectID.EffectCompareType)) effectCommand.Execute();
            AddLast(effectCommand);
        }

        public bool Contains(EffectID effectID, EffectID.CompareType compareType)
        {
            return this.Contains(c => c.EffectID.Compare(effectID, compareType));
        }

        public new EffectCommand First(EffectID effectID, EffectID.CompareType compareType)
        {
            return this.FirstOrDefault(e => e.EffectID.Compare(effectID, compareType));
        }

        private new void Remove(LinkedListNode<EffectCommand> node)
        {
            var command = node.Value;
            base.Remove(node);
            if (command == null) { return; }
            if (!Contains(node.Value.EffectID, node.Value.EffectID.EffectCompareType)) { command.Release(); }
        }

        public bool Remove(EffectID effectID, bool removeAll = false)
        {
            bool removed = false;
            if (removeAll)
            {
                this.ForEachNodes(node =>
                {
                    if (!node.Value.EffectID.Compare(effectID, effectID.ObjectCompareType)) { return; }

                    removed = true;
                    Remove(node);
                });
            }
            else
            {
                var node = this.NodeFirst(e =>
                    e.Value.EffectID.Compare(effectID, e.Value.EffectID.ObjectCompareType));
                removed = node != null;
                Remove(node);
            }

            return removed;
        }

        public new void Clear()
        {
            this.ForEachNodes(Remove);
        }
    }

    public class EffectManager : SingleMono<EffectManager>
    {
        private readonly Dictionary<Enemy, EffectCommandList> _effectCommands = new();

        public void Add(EffectCommand effectCommand)
        {
            var enemy = effectCommand.Enemy;

            if (!_effectCommands.TryGetValue(enemy, out var list))
            {
                list = new EffectCommandList();
                _effectCommands.Add(enemy, list);
            }

            list.Add(effectCommand);
        }

        public bool Contains(Enemy enemy, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.Contains(effectID, compareType) ?? false;
        }

        public EffectCommand First(Enemy enemy, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.First(effectID, compareType);
        }

        public bool Remove(Enemy enemy, EffectID effectID, bool removeAll = false)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.Remove(effectID, removeAll) ?? false;
        }

        public IEnumerable<Enemy> Remove(EffectID effectID, bool removeAll = false)
        {
            var effectRemovedEnemies = new HashSet<Enemy>(25);

            foreach (var (enemy, list) in _effectCommands)
            {
                if (list.Remove(effectID, removeAll))
                {
                    effectRemovedEnemies.Add(enemy);
                }
            }

            return effectRemovedEnemies;
        }

        public void Clear(Enemy enemy)
        {
            if (!_effectCommands.TryGetValue(enemy, out var list)) { return; }

            list?.Clear();
        }

        private void Update()
        {
            RemoveExpiredCommands();
        }

        private void RemoveExpiredCommands()
        {
            foreach (var keyValuePair in _effectCommands)
            {
                var list = keyValuePair.Value;
                if (list == null || list.Count < 1) { continue; }

                list.ForEachNodes(node =>
                {
                    if (node.Value == null || node.Value.EndTime <= Time.time) { list.Remove(node); }
                });
            }
        }
    }
}