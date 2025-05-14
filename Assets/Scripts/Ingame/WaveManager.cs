using System;
using System.Collections;
using SO;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Random = UnityEngine.Random;

namespace Ingame
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool<Enemy> enemyPool;
        private Collider _spawnArea;

        private void Awake()
        {
            _spawnArea = GetComponent<Collider>();
        }

        public void StartWave(StageWaveDataSO waveData, Action onClear)
        {
            StartCoroutine(PlayWave(waveData, onClear));
        }

        private IEnumerator PlayWave(StageWaveDataSO waveData, Action onClear)
        {
            if (waveData.EnemyCount < 1)
            {
                onClear?.Invoke();
                yield break;
            }
            
            var spawnDelay = new WaitForSeconds(Mathf.Max(1 / waveData.GeneratePerSec, 0.2f));
            int enemyCount = waveData.EnemyCount;
            int enemyKillCount = 0;
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = enemyPool.Get();
                enemy.SetStats(waveData.Health, waveData.Damage, waveData.Speed);
                InitEnemyTransform(enemy.transform);
                enemy.OnDie += () =>
                {
                    enemyPool.Return(enemy);
                    if(++enemyKillCount == enemyCount) onClear();
                };
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