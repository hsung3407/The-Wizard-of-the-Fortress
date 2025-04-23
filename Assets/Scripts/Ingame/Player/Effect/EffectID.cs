using System;
using UnityEngine;

namespace Ingame.Player.Effect
{
    [Serializable]
    public struct EffectIDData
    {
        [SerializeField] public string effectGroupName;
        [SerializeField] public string effectName;

        [SerializeField] public EffectID.CompareType objectCompareType;
        [SerializeField] public EffectID.CompareType effectCompareType;

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
}