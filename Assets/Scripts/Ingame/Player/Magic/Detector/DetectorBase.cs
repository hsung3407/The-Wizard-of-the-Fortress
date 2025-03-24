using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public abstract class DetectorBase : MonoBehaviour
    {
        private Dictionary<Mob, int> DetectedMobs {get; set;} = new Dictionary<Mob, int>();

        private Action<Mob> OnDetected { get; set; }
        private Action<Mob> OnReleased { get; set; }
        public void RegisterOnDetected(Action<Mob> onDetected) => OnDetected = onDetected;
        public void RegisterOnReleased(Action<Mob> onReleased) => OnReleased = onReleased;

        //true = 감지가 해제 되기 전에 재 감지 되었을 때 효과를 중복 적용 가능
        [SerializeField] private bool isDuplicateDetectable;

        protected void OnDetect(Mob mob)
        {
            if (!DetectedMobs.TryAdd(mob, 1))
            {
                if (!isDuplicateDetectable) return;
                DetectedMobs[mob]++;
            }

            OnDetected(mob);
        }

        protected void OnRelease(Mob mob)
        {
            if (!DetectedMobs.ContainsKey(mob))
            {
                DetectedMobs[mob]--;
                if (DetectedMobs[mob] == 0) DetectedMobs.Remove(mob);
            }

            OnReleased(mob);
        }

        protected void OnDisable()
        {
            if(OnReleased != null)
            {
                foreach (var pair in DetectedMobs)
                {
                    for (var i = 0; i < pair.Value; i++)
                    {
                        OnReleased.Invoke(pair.Key);
                    }
                }
            }
            DetectedMobs.Clear();
        }
    }
}