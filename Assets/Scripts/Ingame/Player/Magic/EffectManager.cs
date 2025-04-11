using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Utility;
using Utility.SingleTon;

namespace Ingame.Player
{
    public class EffectID
    {
        private readonly string _effectGroupName;
        private readonly string _effectName;

        public EffectID(string effectGroupName, string effectName)
        {
            _effectGroupName = effectGroupName;
            _effectName = effectName;
        }

        public static bool operator ==(EffectID a, EffectID b)
        {
            if (a == null && b == null)
                return true;
            else if (a != null && b == null)
                return false;
            else if (a == null) return false;

            bool effectGroupNameIsSame = a._effectGroupName == b._effectGroupName;
            bool effectNameIsSame = a._effectName == b._effectName;
            return effectGroupNameIsSame && effectNameIsSame;
        }

        public static bool operator !=(EffectID a, EffectID b)
        {
            return !(a == b);
        }
    }

    public abstract class EffectCommand
    {
        public readonly EffectID EffectID;
        public readonly Enemy Enemy;

        public EffectCommand(EffectID effectID, Enemy enemy)
        {
            EffectID = effectID;
            Enemy = enemy;
        }

        public abstract void Execute();
        public abstract void Release();
    }

    public class EffectCommandInfo
    {
        [NotNull] public readonly EffectCommand EffectCommand;
        public float EndTime { get; private set; }

        public EffectCommandInfo(EffectCommand effectCommand, float endTime)
        {
            EffectCommand = effectCommand;
            EndTime = endTime;
        }

        public void ResetEndTime(float duration)
        {
            EndTime = Time.time + duration;
        }
    }

    public class EffectManager : SingleMono<EffectManager>
    {
        //float is end Time of effect command duration
        private readonly Dictionary<Enemy, LinkedList<EffectCommandInfo>> _effectCommandInfos = new();

        public void Add(EffectCommand effectCommand, float duration)
        {
            var enemy = effectCommand.Enemy;

            if (_effectCommandInfos.TryGetValue(enemy, out var list))
            {
                var duplication = list.FirstOrDefault(commandInfo =>
                    commandInfo.EffectCommand.EffectID == effectCommand.EffectID);
                if (duplication != null)
                {
                    duplication.ResetEndTime(duration);
                    return;
                }
            }
            else
            {
                list = new LinkedList<EffectCommandInfo>();
                _effectCommandInfos.Add(enemy, list);
            }

            effectCommand.Execute();
            list.AddLast(new EffectCommandInfo(effectCommand, Time.time + duration));
        }

        private static void Remove(LinkedListNode<EffectCommandInfo> node)
        {
            node.Value.EffectCommand.Release();
            node.List.Remove(node);
        }

        public bool Remove(Enemy enemy, EffectID effectID, bool removeAll = false)
        {
            bool removed = false;
            var list = _effectCommandInfos[enemy];
            list.ForEachNodes(node =>
            {
                if (node.Value.EffectCommand.EffectID != effectID || (!removeAll && removed)) { return; }

                removed = true;
                Remove(node);
            });
            return removed;
        }

        public IEnumerable<Enemy> Remove(EffectID effectID, bool removeAll = false)
        {
            var effectRemovedEnemies = new Queue<Enemy>(25);

            foreach (var (key, value) in _effectCommandInfos)
            {
                bool removed = false;
                value.ForEachNodes(node =>
                {
                    if (node.Value.EffectCommand.EffectID != effectID || (!removeAll && removed)) { return; }

                    removed = true;
                    effectRemovedEnemies.Enqueue(key);
                    Remove(node);
                });
            }

            return effectRemovedEnemies;
        }

        public void Clear(Enemy enemy)
        {
            if (!_effectCommandInfos.TryGetValue(enemy, out var list)) { return; }

            list.ForEachNodes(Remove);
            _effectCommandInfos.Remove(enemy);
        }

        private void Update()
        {
            RemoveExpiredCommands();
        }

        private void RemoveExpiredCommands()
        {
            foreach (var keyValuePair in _effectCommandInfos)
            {
                var list = keyValuePair.Value;
                if (list == null || list.Count < 1) { continue; }

                list.ForEachNodes(node =>
                {
                    if (node.Value.EndTime <= Time.time) { Remove(node); }
                });
            }
        }
    }
}