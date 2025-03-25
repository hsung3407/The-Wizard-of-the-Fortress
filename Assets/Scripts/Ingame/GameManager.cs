using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ingame;
using SO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float waveDelay;
    private StageWaveDataSO[] _waveData;
    private int _waveIndex;


    [SerializeField] private MobGenerator mobGenerator;

    private void Awake()
    {
        _waveData = Resources.LoadAll<StageWaveDataSO>($"Stage/{StageInfo.stageIndex}");
        Debug.Log($"{_waveData.Length}");
    }

    private void Start()
    {
        _waveIndex = 0;
        StartCoroutine(PlayWave());
    }

    private IEnumerator PlayWave()
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
                mobGenerator.SpawnMob();
                yield return spawnDelay;
            }

            Debug.Log($"Wave End : {_waveIndex}");
            
            yield return new WaitForSeconds(waveDelay);
        }
        
        Debug.Log($"Stage Clear");
    }
}