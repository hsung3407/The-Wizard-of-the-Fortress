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
    public class EffectID
    {
        [SerializeField] private string effectGroupName;
        [SerializeField] private string effectName;
        private int _ownerID;

        [field: SerializeField] public CompareType ObjectCompareType { get; private set; }
        [field: SerializeField] public CompareType EffectCompareType { get; private set; }
        
        public EffectID(EffectID effectID, int ownerID)
        {
            effectGroupName = effectID.effectGroupName;
            effectName = effectID.effectName;
            _ownerID = ownerID;
            ObjectCompareType = effectID.ObjectCompareType;
            EffectCompareType = effectID.EffectCompareType;
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
            bool checkGroup = a?.effectGroupName == b?.effectGroupName;
            bool checkName = a?.effectName == b?.effectName;
            bool checkOwner = a?._ownerID == b?._ownerID;
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
            return $"[{effectGroupName}]{effectName}";
        }
    }

    public abstract class EffectCommand : Object
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
                if (list.Remove(effectID, removeAll))
                {
                    effectRemovedEnemies.Add(effectedObject);
                }
            }

            return effectRemovedEnemies;
        }

        public void Clear(object effectedObject)
        {
            if (!_effectCommands.TryGetValue(effectedObject, out var list)) { return; }

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
                    if (node.Value?.IsExpired() ?? true) { list.Remove(node); }
                });
            }
        }
    }
}