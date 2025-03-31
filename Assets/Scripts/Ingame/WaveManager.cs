using System;
using System.Collections;
using SO;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Ingame
{
    public class WaveManager : MonoBehaviour
    {
        private EnemyPool _enemyPool;
        private Collider _spawnArea;

        private void Awake()
        {
            _enemyPool = GetComponent<EnemyPool>();
            _spawnArea = GetComponent<Collider>();
            // _castleWallTr = GameObject.FindWithTag("CastleWall").transform;
        }

        public void StartWave(StageWaveDataSO waveData, Action onClear)
        {
            StartCoroutine(PlayWave(waveData, onClear));
        }

        private IEnumerator PlayWave(StageWaveDataSO waveData, Action onClear)
        {
            var spawnDelay = new WaitForSeconds(1 / waveData.GeneratePerSec);
            int enemyCount = waveData.EnemyCount;
            int enemyKillCount = 0;
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = _enemyPool.Get(StaticStageInfo.MobType);
                InitEnemyTransform(enemy.transform);
                enemy.Init((() =>
                {
                    _enemyPool.Return(enemy);
                    if(++enemyKillCount == enemyCount) onClear();
                }));
                yield return spawnDelay;
            }
        }

        private void InitEnemyTransform(Transform tr)
        {
            SetPositionInBounds(tr);
            SetRotation(tr);
        }

        private void SetPositionInBounds(Transform tr)
        {
            tr.position = new Vector3(Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x),
                Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y),
                Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z));
        }

        private void SetRotation(Transform tr)
        {
            tr.transform.rotation = transform.rotation;
        }
    }
}