using UnityEngine;

namespace Ingame.Player.Magic.Detector
{
    public class FieldEnterDetector : DetectorBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Mob")) return;
            var mob = other.GetComponent<Mob>();
            if (mob == null) return;
            OnDetect(mob);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Mob")) return;
            var mob = other.GetComponent<Mob>();
            if (mob == null) return;
            OnRelease(mob);
        }
    }
}