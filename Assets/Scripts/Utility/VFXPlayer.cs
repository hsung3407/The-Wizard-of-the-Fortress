using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Utility
{
    public class VFXPlayer : MonoBehaviour
    {
        [SerializeField] private bool playOnEnable;
        [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        [SerializeField] private List<VisualEffect> visualEffects = new List<VisualEffect>();

        private void OnEnable()
        {
            if(playOnEnable) Play();
        }

        public void Play()
        {
            foreach (var system in particleSystems)
            {
                system.Play();
            }
            foreach (var visualEffect in visualEffects)
            {
                visualEffect.Play();
            }
        }
    }
}
