using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.SingleTon;

namespace Utility.Sound
{
    public enum SFXType
    {
        MonsterHit,
        WallCrash,
        WallBreak,
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingleMono<SoundManager>
    {
        private AudioSource _audioSource;
        private Dictionary<SFXType, AudioClip> _sfxClips = new();
        private Dictionary<SFXType, float> _playedTime = new();
        
        private Coroutine _musicChangeCoroutine;

        protected override void Awake()
        {
            canBeDestroy = false;
            
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            
            var data = Resources.LoadAll<SFXDataSO>("SFXDataSO");
            foreach (var sfxData in data)
            {
                _sfxClips.Add(sfxData.Type, sfxData.Clip);
                _playedTime.Add(sfxData.Type, 0f);
            }
        }

        public void PlaySFX(AudioClip clip, float volume)
        {
            if(!clip) return;
            
            _audioSource.PlayOneShot(clip, volume);
        }
        
        public void PlaySFX(SFXType type, float volume)
        {
            if(_playedTime[type] >= Time.time) return;
            _playedTime[type] = Time.time;
            _audioSource.PlayOneShot(_sfxClips[type], volume);
        }

        public void PlayMusic(AudioClip clip, float volume)
        {
            if(!clip) return;
            
            if(_musicChangeCoroutine != null) StopCoroutine(_musicChangeCoroutine);
            _musicChangeCoroutine = StartCoroutine(MusicChange(clip, volume));
        }

        private IEnumerator MusicChange(AudioClip clip, float volume)
        {
            float changTime = 1f;
            
            if(_audioSource.clip && _audioSource.isPlaying) yield return FadeOut(changTime / 2, volume);
            _audioSource.clip = clip;
            yield return FadeIn(changTime / 2, volume);

            _musicChangeCoroutine = null;
        }

        private IEnumerator FadeOut(float time, float maxVolume)
        {
            for (float i = 0; i < time; i+= Time.deltaTime)
            {
                _audioSource.volume = maxVolume * (1 - i / time);
                yield return null;
            }
            _audioSource.Stop();
        }
        
        private IEnumerator FadeIn(float time, float maxVolume)
        {
            _audioSource.Play();
            for (float i = 0; i < time; i+= Time.deltaTime)
            {
                _audioSource.volume = maxVolume * i / time;
                yield return null;
            }
        }
    }
}