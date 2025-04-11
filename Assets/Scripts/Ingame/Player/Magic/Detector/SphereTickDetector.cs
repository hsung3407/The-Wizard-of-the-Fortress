using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player.Magic.Detector
{
    public class SphereTickDetector : DetectorBase
    {
        [FormerlySerializedAs("range")] [SerializeField]
        private float radius = 1;

        [SerializeField] private float tickDelay = 1;

        private void Start()
        {
            StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            var delay = new WaitForSeconds(tickDelay);

            while (true)
            {
                foreach (var detectedCollider in Physics.OverlapSphere(transform.position,
                             radius,
                             LayerMask.GetMask("Enemy"))) { Detect(detectedCollider.GetComponent<Enemy>()); }

                yield return delay;
            }
        }
    }
}