using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public class FieldTickDetector : DetectorBase
    {
        [SerializeField] private float tickDelay = 1;

        private readonly List<Enemy> _enteredEnemies = new List<Enemy>();

        private void Start()
        {
            StartCoroutine(Tick());
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            _enteredEnemies.Add(enemy);
        }

        private IEnumerator Tick()
        {
            var delay = tickDelay;

            while (true)
            {
                foreach (var enteredEnemy in _enteredEnemies)
                {
                    OnDetect(enteredEnemy);
                }

                yield return delay;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            _enteredEnemies.Remove(enemy);
            OnRelease(enemy);
        }
    }
}