using System.Collections.Generic;
using System.Linq;
using Utility;
using Utility.SingleTon;
using Object = UnityEngine.Object;

namespace Ingame.Player.Effect
{
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
        private readonly Dictionary<Object, EffectCommandList> _effectCommands = new();

        public void Add(EffectCommand effectCommand)
        {
            Object target = effectCommand.Target;
            if (!_effectCommands.TryGetValue(target, out var list))
            {
                list = new EffectCommandList();
                _effectCommands.Add(target, list);
            }

            list.Add(effectCommand);
        }

        public bool Contains(Object target, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(target, out var list);
            return list?.Contains(effectID, compareType) ?? false;
        }

        public EffectCommand First(Object target, EffectID effectID, EffectID.CompareType compareType)
        {
            _effectCommands.TryGetValue(target, out var list);
            return list?.First(effectID, compareType);
        }

        public bool Remove(Object target, EffectID effectID, bool removeAll = false)
        {
            _effectCommands.TryGetValue(target, out var list);
            return list?.Remove(effectID, removeAll) ?? false;
        }

        public IEnumerable<Object> Remove(EffectID effectID, bool removeAll = false)
        {
            var removedTargets = new HashSet<Object>(25);

            foreach (var (target, list) in _effectCommands)
            {
                if (list.Remove(effectID, removeAll)) { removedTargets.Add(target); }
            }

            return removedTargets;
        }

        public void Clear(Object target)
        {
            if (!_effectCommands.TryGetValue(target, out var list)) { return; }
            
            list?.Clear();
            _effectCommands[target] = null;
            _effectCommands.Remove(target);
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