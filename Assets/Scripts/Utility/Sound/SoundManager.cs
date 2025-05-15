using System.Collections.Generic;
using UnityEngine;
using Utility.SingleTon;

namespace Utility.Sound
{
    public enum SFXType
    {
        MonsterHit,
        WallCrash
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingleMono<SoundManager>
    {
        private AudioSource _audioSource;
        private Dictionary<SFXType, AudioClip> _sfxClips = new();
        private Dictionary<SFXType, float> _playedTime = new();

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            var data = Resources.LoadAll<SFXDataSO>("SFXDataSO");
            foreach (var sfxData in data)
            {
                _sfxClips.Add(sfxData.Type, sfxData.Clip);
                _playedTime.Add(sfxData.Type, 0f);
            }
            
            canBeDestroy = false;
        }

        public void PlaySFX(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
        
        public void PlaySFX(SFXType type, float volume)
        {
            if(_playedTime[type] >= Time.time) return;
            _playedTime[type] = Time.time;
            _audioSource.PlayOneShot(_sfxClips[type], volume);
        }
    }
}