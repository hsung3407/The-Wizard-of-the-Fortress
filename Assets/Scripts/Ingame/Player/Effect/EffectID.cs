using System;
using UnityEngine;

namespace Ingame.Player.Effect
{
    //effect id의 성능적 효율을 위해 별도로 Inspector설정용 구조체 추가
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

    //편리하고 효율적인 각 효과별 구분을 위한 ID
    public readonly struct EffectID
    {
        private readonly string _effectGroupName;
        private readonly string _effectName;
        private readonly int _ownerID;
        
        //자신의 고유 객체를 구분하는 비교 형식
        //(사용 예시: effect name이 겹치는 이펙트가 이미 적용되어 있는 경우 해당 이펙트 객체를 저장한다/안 한다 등)
        public readonly CompareType ObjectCompareType;
        //자신의 고유 객체와 별개로 효과 적용 여부를 판단하는 비교 형식
        //(사용 예시: 같은 Ice그룹의 효과가 이미 적용되어 있는 경우 추가로 적용시킨다/안 시킨다 등)
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