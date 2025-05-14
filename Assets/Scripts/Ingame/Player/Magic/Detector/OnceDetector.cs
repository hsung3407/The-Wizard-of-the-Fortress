using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Player.Magic.Detector
{
    public class OnceDetector : DetectorBase
    {
        private Coroutine _coroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy>();
            Detect(enemy);

            if (_coroutine == null) _coroutine = StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return null;
            GetComponent<Collider>()
                .enabled = false;
        }
    }
}