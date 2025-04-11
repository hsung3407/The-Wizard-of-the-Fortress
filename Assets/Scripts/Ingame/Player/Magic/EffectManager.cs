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

        public EffectID(string effectGroupName, string effectName)
        {
            this.effectGroupName = effectGroupName;
            this.effectName = effectName;
        }

        public static bool operator ==(EffectID a, EffectID b)
        {
            bool effectGroupNameIsSame = a?.effectGroupName == b?.effectGroupName;
            bool effectNameIsSame = a?.effectName == b?.effectName;
            return effectGroupNameIsSame && effectNameIsSame;
        }

        public static bool operator !=(EffectID a, EffectID b)
        {
            return !(a == b);
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

        public EffectCommand(EffectID effectID, Enemy enemy, float lastDuration)
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

    public class EffectManager : SingleMono<EffectManager>
    {
        private readonly Dictionary<Enemy, LinkedList<EffectCommand>> _effectCommands = new();

        public void Add(EffectCommand effectCommand)
        {
            var enemy = effectCommand.Enemy;

            if (!_effectCommands.TryGetValue(enemy, out var list))
            {
                list = new LinkedList<EffectCommand>();
                _effectCommands.Add(enemy, list);
            }

            effectCommand.Execute();
            list.AddLast(effectCommand);
        }

        public EffectCommand First(Enemy enemy, EffectID effectID)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.FirstOrDefault(e => e.EffectID == effectID);
        }

        public bool Contains(Enemy enemy, EffectID effectID)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.Select(e => e.EffectID)
                .Contains(effectID) ?? false;
        }

        private static void Remove(LinkedListNode<EffectCommand> node)
        {
            node.Value.Release();
            node.List.Remove(node);
        }

        public bool Remove(Enemy enemy, EffectID effectID, bool removeAll = false)
        {
            bool removed = false;
            _effectCommands.TryGetValue(enemy, out var list);
            list?.ForEachNodes(node =>
            {
                if (node.Value.EffectID != effectID || (!removeAll && removed)) { return; }

                removed = true;
                Remove(node);
            });
            return removed;
        }

        public IEnumerable<Enemy> Remove(EffectID effectID, bool removeAll = false)
        {
            var effectRemovedEnemies = new Queue<Enemy>(25);

            foreach (var (key, value) in _effectCommands)
            {
                bool removed = false;
                value.ForEachNodes(node =>
                {
                    if (node.Value.EffectID != effectID || (!removeAll && removed)) { return; }

                    removed = true;
                    effectRemovedEnemies.Enqueue(key);
                    Remove(node);
                });
            }

            return effectRemovedEnemies;
        }

        public void Clear(Enemy enemy)
        {
            if (!_effectCommands.TryGetValue(enemy, out var list)) { return; }

            list.ForEachNodes(Remove);
            _effectCommands.Remove(enemy);
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
                    if (node.Value.EndTime <= Time.time)
                    {
                        Remove(node);
                    }
                });
            }
        }
    }
}