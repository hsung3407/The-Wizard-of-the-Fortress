using System;
using System.Collections.Generic;
using Ingame.Player.Effect;
using Ingame.Player.Effect.Command;
using Ingame.Player.Modifier;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Modifier
{
    public class Effector : ModifierBase
    {
        [SerializeField] private EffectIDData effectIDData;
        private EffectID _effectID;
        
        [SerializeField] private TimeLimitEnemyEffectData effectData;

        private void Awake()
        {
            _effectID = effectIDData.GetEffectID(GetInstanceID());
        }

        public override void Modify([NotNull] Enemy enemy)
        {
            EffectManager.Instance.Add(effectData.GetCommand(_effectID, enemy));
        }

        public override void UnModify([NotNull] Enemy enemy)
        {
            EffectManager.Instance.Remove(enemy, _effectID);
        }
    }
}