using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Player.Magic.Detector
{
    public class FieldEnterDetector : DetectorBase
    {
        private HashSet<Enemy> _detectedEnemies;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            if(_detectedEnemies.Add(enemy)) Detect(enemy);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            if (_detectedEnemies.Remove(enemy)) Release(enemy);
        }

        private void OnDestroy()
        {
            foreach (var detectedEnemy in _detectedEnemies) { Release(detectedEnemy); }
        }
    }
}