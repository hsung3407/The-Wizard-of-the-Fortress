using System;
using System.Collections;
using SO;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.Sound;

namespace Ingame
{
    public class PauseInfo
    {
        public bool PlayerInteractable;
    }
    
    public class IngameManager : MonoBehaviour
    {
        [SerializeField] private Player.Player player;

        [SerializeField] private WaveManager waveManager;
        private StageWaveDataSO[] _waveData;
        private int _waveIndex;

        private PauseInfo _pauseInfo;

        [SerializeField] private AudioClip ingameMusic;
        [SerializeField] private float volume;
        
        private void Awake()
        {
            _waveData = Resources.LoadAll<StageWaveDataSO>($"StageWaveData/Stage {StaticStageInfo.StageIndex}");
        }

        private void Start()
        {
            SoundManager.Instance.PlayMusic(ingameMusic, volume);
            
            _waveIndex = 0;
            StartCoroutine(StartFlow());
            
            player.SetInteractable(false, true);
        }

        public void Pause()
        {
            _pauseInfo.PlayerInteractable = player.Interactable;
            player.SetInteractable(false);

            Time.timeScale = 0;
        }

        public void Resume()
        {
            player.SetInteractable(_pauseInfo.PlayerInteractable);
            Time.timeScale = 1;
        }

        private IEnumerator StartFlow()
        {
            yield return new WaitForSeconds(1f);

            StartCoroutine(IngameFlow());
        }

        private IEnumerator IngameFlow()
        {
            player.SetInteractable(false);
            
            yield return new WaitForSeconds(3f);
            
            yield return StartCoroutine(AbilitySelectFlow());
            
            yield return new WaitForSeconds(2f);

            player.SetInteractable(true);
            WaveStart();
        }

        private IEnumerator AbilitySelectFlow()
        {
            Time.timeScale = 0;
            bool selected = false;
            
            player.PlayerAbility.Select(3, ability =>
            {
                player.PlayerAbility.AddAbility(ability);
                selected = true;
            });
            
            yield return new WaitUntil(()=>selected);
            Time.timeScale = 1;
        }

        private void WaveStart()
        {
            NotificationManager.Instance.NotifyTitle($"Wave {_waveIndex+1} Start");
            
            waveManager.StartWave(_waveData[_waveIndex], WaveClear);
        }

        private void WaveClear()
        {
            NotificationManager.Instance.NotifyTitle($"Wave {_waveIndex+1} Clear");

            if (++_waveIndex == _waveData.Length) StageClear();
            else StartCoroutine(IngameFlow());
        }

        private void StageClear()
        {
            NotificationManager.Instance.NotifyTitle("Stage Clear");
        }
    }
}