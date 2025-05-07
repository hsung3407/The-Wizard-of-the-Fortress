using System.Collections;
using SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    public class IngameManager : MonoBehaviour
    {
        [SerializeField] private Player.Player player;
        private StageWaveDataSO[] _waveData;
        private int _waveIndex;
        
        [SerializeField] private WaveManager waveManager;

        private void Awake()
        {
            _waveData = Resources.LoadAll<StageWaveDataSO>($"StageWaveData/Stage {StaticStageInfo.StageIndex}");
        }

        private void Start()
        {
            _waveIndex = 0;
            StartCoroutine(StartFlow());
        }

        private IEnumerator StartFlow()
        {
            yield return new WaitForSeconds(1f);

            StartCoroutine(IngameFlow());
        }

        private IEnumerator IngameFlow()
        {
            // bool selected = false;
            //
            // player.PlayerAbility.Select(3, ability =>
            // {
            //     player.PlayerAbility.AddAbility(ability);
            //     selected = true;
            // });
            
            // yield return new WaitUntil(()=>selected);
            
            yield return new WaitForSeconds(1f);
            
            WaveStart();
        }

        private void WaveStart()
        {
            Debug.Log($"Wave {_waveIndex} Start");
            
            waveManager.StartWave(_waveData[_waveIndex], WaveClear);
        }

        private void WaveClear()
        {
            Debug.Log($"Wave {_waveIndex} Clear");

            if (++_waveIndex == _waveData.Length) StageClear();
            else StartCoroutine(IngameFlow());
        }

        private void StageClear()
        {
            Debug.Log("Stage Clear");
        }
    }
}