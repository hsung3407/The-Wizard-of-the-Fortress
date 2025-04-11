using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public abstract class DetectorBase : MonoBehaviour
    {
        private Action<Enemy> OnDetected { get; set; }
        public void RegisterOnDetected(Action<Enemy> onDetected) => OnDetected = onDetected;

        protected void OnDetect(Enemy enemy)
        {
            if (enemy == null) return;

            OnDetected(enemy);
        }
    }
}