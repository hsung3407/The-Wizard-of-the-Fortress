using System;
using System.Collections;
using SO;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Ingame
{
    public class EnemySpawner : MonoBehaviour
    {
        private Collider _spawnArea;
        private Transform _castleWallTr;
        
        private void Awake()
        {
            _spawnArea = GetComponent<Collider>();
            _castleWallTr = GameObject.FindWithTag("CastleWall").transform;
        }
        
        public void SpawnEnemy()
        {
            var go = ObjectPool.Instance.Get(StaticStageInfo.MobType);
            go.transform.position = new Vector3(Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x),
                Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y),
                Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z));
            go.transform.rotation = transform.rotation;
        }
    }
}