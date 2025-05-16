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
    
    public class SoundManager : SingleMono<SoundManager>
    {
        private AudioSource _sfxAudioSource;
        private AudioSource _musicAudioSource;
        private Dictionary<SFXType, (AudioClip, float)> _sfxClips = new();
        private Dictionary<SFXType, float> _playedTime = new();
        
        private Coroutine _musicChangeCoroutine;

        protected override void Awake()
        {
            canBeDestroy = false;
            
            base.Awake();
            _sfxAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource.loop = true;
            
            var data = Resources.LoadAll<SFXDataSO>("SFXDataSO");
            foreach (var sfxData in data)
            {
                _sfxClips.Add(sfxData.Type, (sfxData.Clip, sfxData.Volume));
                _playedTime.Add(sfxData.Type, 0f);
            }
        }

        public void PlaySFX(AudioClip clip, float volume)
        {
            if(!clip) return;
            
            _sfxAudioSource.PlayOneShot(clip, volume);
        }
        
        public void PlaySFX(SFXType type, float volume)
        {
            if(_playedTime[type] >= Time.time) return;
            _playedTime[type] = Time.time;
            _sfxAudioSource.PlayOneShot(_sfxClips[type].Item1, _sfxClips[type].Item2 * volume);
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
            
            if(_musicAudioSource.clip && _musicAudioSource.isPlaying) yield return FadeOut(changTime / 2, volume);
            _musicAudioSource.clip = clip;
            yield return FadeIn(changTime / 2, volume);

            _musicChangeCoroutine = null;
        }

        private IEnumerator FadeOut(float time, float maxVolume)
        {
            for (float i = 0; i < time; i+= Time.deltaTime)
            {
                _musicAudioSource.volume = maxVolume * (1 - i / time);
                yield return null;
            }
            _musicAudioSource.Stop();
        }
        
        private IEnumerator FadeIn(float time, float maxVolume)
        {
            _musicAudioSource.Play();
            for (float i = 0; i < time; i+= Time.deltaTime)
            {
                _musicAudioSource.volume = maxVolume * i / time;
                yield return null;
            }
        }
    }
}