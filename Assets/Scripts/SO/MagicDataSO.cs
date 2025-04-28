using System;
using System.Collections.Generic;
using Ingame.Player.Effect;
using Ingame.Player.Predictor;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Ingame.Player
{
    public class MagicStatsModifier
    {
        public float AdditionalDamage = 0;
        public float DamageMultiplier = 1;
        public float AdditionalManaCost = 0;
        public float ManaCostMultiplier = 1;

        public MagicStats Modify(MagicStats original)
        {
            var damage = (original.Damage + AdditionalDamage) * DamageMultiplier;
            var manaCost = (original.ManaCost + AdditionalManaCost) * ManaCostMultiplier;
            return new MagicStats(damage, manaCost);
        }
    }

    [Serializable]
    public struct MagicStats
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float ManaCost { get; private set; }

        public MagicStats(float damage, float manaCost)
        {
            Damage = damage;
            ManaCost = manaCost;
        }
    }

    [CreateAssetMenu(fileName = "MagicDataSO", menuName = "Scriptable Objects/MagicDataSO")]
    public class MagicDataSO : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; private set; }
        [field: SerializeField] public string MagicName { get; private set; }
        [field: SerializeField] public string Command { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        [field: SerializeField] public MagicController MagicObject { get; private set; }
        [field: SerializeField] public PredictorManager.PredictorType PredictorType { get; private set; }
        [field: SerializeField] public float PredictRange { get; private set; }
        [field: SerializeField] public MagicStats MagicStats { get; private set; }
    }
}