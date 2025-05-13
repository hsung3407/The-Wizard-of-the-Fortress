using UnityEngine;
using Utility.SingleTon;

namespace Utility
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingleMono<SoundManager>
    {
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySFX(AudioClip source)
        {
            _audioSource.PlayOneShot(source);
        }
    }
}