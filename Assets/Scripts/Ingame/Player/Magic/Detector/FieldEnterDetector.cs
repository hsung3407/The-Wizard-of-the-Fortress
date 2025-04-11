using UnityEngine;

namespace Ingame.Player.Magic.Detector
{
    public class FieldEnterDetector : DetectorBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            OnDetect(enemy);
        }
    }
}