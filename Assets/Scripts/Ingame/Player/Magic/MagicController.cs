using System;
using Ingame.Player.Magic.Detector;
using Ingame.Player.Modifier;
using Ingame.Player.Predictor;
using UnityEngine;

namespace Ingame.Player
{
    public class MagicController : MonoBehaviour
    {
        private DetectorBase detector;
        private ModifierBase modifier;

        private void Awake()
        {
            detector = GetComponentInParent<DetectorBase>();
            modifier = GetComponentInParent<ModifierBase>();
        }

        private void Start()
        {
            if (!detector || !modifier) return;
            detector.RegisterOnDetected(modifier.Modify);
            detector.RegisterOnReleased(modifier.UnModify);
        }
    }
}