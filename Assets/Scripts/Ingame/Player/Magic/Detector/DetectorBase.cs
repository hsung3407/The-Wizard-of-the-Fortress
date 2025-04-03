using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public abstract class DetectorBase : MonoBehaviour
    {
        private Action<Enemy> OnDetected { get; set; }
        private Action<Enemy> OnReleased { get; set; }
        public void RegisterOnDetected(Action<Enemy> onDetected) => OnDetected = onDetected;
        public void RegisterOnReleased(Action<Enemy> onReleased) => OnReleased = onReleased;

        protected void OnDetect(Enemy enemy)
        {
            if (enemy == null) return;

            OnDetected(enemy);
        }

        protected void OnRelease(Enemy enemy)
        {
            if (enemy == null) return;

            OnReleased(enemy);
        }
    }
}