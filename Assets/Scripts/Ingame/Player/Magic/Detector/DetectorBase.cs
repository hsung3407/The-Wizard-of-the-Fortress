using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public abstract class DetectorBase : MonoBehaviour
    {
        public event Action<Enemy> OnDetect;
        public event Action<Enemy> OnRelease;

        protected void Detect(Enemy enemy)
        {
            if (OnDetect == null || enemy == null) return;
            OnDetect.Invoke(enemy);
        }

        protected void Release(Enemy enemy)
        {
            if (OnRelease == null || enemy == null) return;
            OnRelease.Invoke(enemy);
        }
    }
}