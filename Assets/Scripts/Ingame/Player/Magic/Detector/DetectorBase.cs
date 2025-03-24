using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public abstract class DetectorBase : MonoBehaviour
    {
        private Dictionary<Enemy, int> DetectedMobs {get; set;} = new Dictionary<Enemy, int>();

        private Action<Enemy> OnDetected { get; set; }
        private Action<Enemy> OnReleased { get; set; }
        public void RegisterOnDetected(Action<Enemy> onDetected) => OnDetected = onDetected;
        public void RegisterOnReleased(Action<Enemy> onReleased) => OnReleased = onReleased;

        //true = 감지가 해제 되기 전에 재 감지 되었을 때 효과를 중복 적용 가능
        [SerializeField] private bool isDuplicateDetectable;

        protected void OnDetect(Enemy enemy)
        {
            if (!DetectedMobs.TryAdd(enemy, 1))
            {
                if (!isDuplicateDetectable) return;
                DetectedMobs[enemy]++;
            }

            OnDetected(enemy);
        }

        protected void OnRelease(Enemy enemy)
        {
            if (!DetectedMobs.ContainsKey(enemy))
            {
                DetectedMobs[enemy]--;
                if (DetectedMobs[enemy] == 0) DetectedMobs.Remove(enemy);
            }

            OnReleased(enemy);
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