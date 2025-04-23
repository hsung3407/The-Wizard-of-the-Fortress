using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Utility.SingleTon;
using Object = UnityEngine.Object;

namespace Ingame.Player
{
    [Serializable]
    public struct EffectIDData
    {
        [SerializeField] private string effectGroupName;
        [SerializeField] private string effectName;

        [SerializeField] private EffectID.CompareType objectCompareType;
        [SerializeField] private EffectID.CompareType effectCompareType;

        public EffectID GetEffectID(int ownerID)
        {
            return new EffectID(effectGroupName, effectName, ownerID, objectCompareType, effectCompareType);
        }
    }

    public readonly struct EffectID
    {
        private readonly string _effectGroupName;
        private readonly string _effectName;
        private readonly int _ownerID;
        public readonly CompareType ObjectCompareType;
        public readonly CompareType EffectCompareType;

        public EffectID(string effectGroupName, string effectName, int ownerID, CompareType objectCompareType,
            CompareType effectCompareType)
        {
            _effectGroupName = effectGroupName;
            _effectName = effectName;
            _ownerID = ownerID;
            ObjectCompareType = objectCompareType;
            EffectCompareType = effectCompareType;
        }

        public enum CompareType
        {
            All,
            GroupName,
            EffectName,
            OnlyName,
        }

        public bool Compare(EffectID other, CompareType compareType)
        {
            return Compare(this, other, compareType);
        }

        public static bool Compare(EffectID a, EffectID b, CompareType compareType)
        {
            bool checkGroup = a._effectGroupName == b._effectGroupName;
            bool checkName = a._effectName == b._effectName;
            bool checkOwner = a._ownerID == b._ownerID;
            return compareType switch
            {
                CompareType.All => checkGroup && checkName && checkOwner,
                CompareType.GroupName => checkGroup,
                CompareType.EffectName => checkName,
                CompareType.OnlyName => checkGroup && checkName,
                _ => throw new ArgumentOutOfRangeException(nameof(compareType), compareType, null)
            };
        }

        public override string ToString()
        {
            return $"[{_effectGroupName}]{_effectName}";
        }
    }

    public abstract class EffectCommand
    {
        public readonly EffectID EffectID;
        public readonly Object Obj;

        protected EffectCommand(EffectID effectID, Object obj)
        {
            EffectID = effectID;
            Obj = obj;
        }

        public abstract void Execute();
        public abstract void Release();

        public abstract bool IsExpired();
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
            if (node == null) return;

            var command = node.Value;
            base.Remove(node);
            if (command != null && !Contains(command.EffectID, command.EffectID.EffectCompareType))
            {
                command.Release();
            }
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
        private readonly Dictionary<object, EffectCommandList> _effectCommands = new();

        public void Add(EffectCommand effectCommand)
        {
            var enemy = effectCommand.Obj;

            if (!_effectCommands.TryGetValue(enemy, out var list))
            {
                list = new EffectCommandList();
                _effectCommands.Add(enemy, list);
            }

            list.Add(effectCommand);
        }

        public bool Contains(object effectedObject, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(effectedObject, out var list);
            return list?.Contains(effectID, compareType) ?? false;
        }

        public EffectCommand First(object effectedObject, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(effectedObject, out var list);
            return list?.First(effectID, compareType);
        }

        public bool Remove(Enemy enemy, EffectID effectID, bool removeAll = false)
        {
            _effectCommands.TryGetValue(enemy, out var list);
            return list?.Remove(effectID, removeAll) ?? false;
        }

        public IEnumerable<object> Remove(EffectID effectID, bool removeAll = false)
        {
            var effectRemovedEnemies = new HashSet<object>(25);

            foreach (var (effectedObject, list) in _effectCommands)
            {
                if (list.Remove(effectID, removeAll)) { effectRemovedEnemies.Add(effectedObject); }
            }

            return effectRemovedEnemies;
        }

        public void Clear(object effectedObject)
        {
            if (!_effectCommands.TryGetValue(effectedObject, out var list)) { return; }
            
            list?.Clear();
            _effectCommands[effectedObject] = null;
            _effectCommands.Remove(effectedObject);
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
                    if (node.Value?.IsExpired() ?? true) { list.Remove(node); }
                });
            }
        }
    }
}