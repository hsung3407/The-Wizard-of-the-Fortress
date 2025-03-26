using System.Collections;
using SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame
{
    public class IngameManager : MonoBehaviour
    {
        [SerializeField] private float waveDelay = 5;
        private StageWaveDataSO[] _waveData;
        private int _waveIndex;
        
        [SerializeField] private EnemySpawner enemySpawner;

        private void Awake()
        {
            _waveData = Resources.LoadAll<StageWaveDataSO>($"StageWaveData/Stage {StageInfo.StageIndex}");
        }

        private void Start()
        {
            _waveIndex = 0;
            StartCoroutine(MainFlow());
        }

        private IEnumerator MainFlow()
        {
            //TODO: Wave Start Notify Display
            int waveDataCount = _waveData.Length;
            for (_waveIndex = 0; _waveIndex < waveDataCount; _waveIndex++)
            {
                Debug.Log($"Wave Start : {_waveIndex}");
            
                var data = _waveData[_waveIndex];
                var spawnDelay = new WaitForSeconds(1 / data.GeneratePerSec);
                int enemyCount = data.EnemyCount;
                for (int i = 0; i < enemyCount; i++)
                {
                    enemySpawner.SpawnEnemy();
                    yield return spawnDelay;
                }

                Debug.Log($"Wave End : {_waveIndex}");
            
                yield return new WaitForSeconds(waveDelay);
            }
        
            Debug.Log($"Stage Clear");
        }
    }
}