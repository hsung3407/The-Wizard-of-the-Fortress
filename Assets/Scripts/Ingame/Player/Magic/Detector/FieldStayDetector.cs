using System;
using UnityEngine;

namespace Ingame.Player.Magic.Detector
{
    public class FieldStayDetector : DetectorBase
    {
        [SerializeField] private float detectDelay = 1;
        private float _lastDetectedTime;

        private void Start()
        {
            _lastDetectedTime = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if(!other.CompareTag("Enemy")) return;
            
            if (Time.time - _lastDetectedTime > detectDelay)
            {
                _lastDetectedTime = Time.time;
                OnDetect(other.GetComponent<Enemy>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Enemy")) return;
            
            OnRelease(other.GetComponent<Enemy>());
        }
    }
}