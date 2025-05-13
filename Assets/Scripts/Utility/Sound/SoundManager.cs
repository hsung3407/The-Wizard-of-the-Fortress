using System.Collections.Generic;
using UnityEngine;
using Utility.SingleTon;

namespace Utility.Sound
{
    public enum SFXType
    {
        MonsterHit,
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingleMono<SoundManager>
    {
        private AudioSource _audioSource;
        private Dictionary<SFXType, AudioClip> _sfxClips = new();

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            var data = Resources.LoadAll<SFXDataSO>("SFXDataSO");
            foreach (var sfxData in data)
            {
                _sfxClips.Add(sfxData.Type, sfxData.Clip);
            }
            
            canBeDestroy = false;
        }

        public void PlaySFX(AudioClip source)
        {
            _audioSource.PlayOneShot(source);
        }
        
        public void PlaySFX(SFXType type)
        {
            _audioSource.PlayOneShot(_audioSource.clip = _sfxClips[type]);
        }
    }
}