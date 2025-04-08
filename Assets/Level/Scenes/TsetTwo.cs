using System;
using System.Collections;
using System.Collections.Generic;
using Ingame;
using UnityEngine;

public class TsetTwo : MonoBehaviour
{
    private EnemyPool _enemyPool;
    private List<Enemy> _enemies = new List<Enemy>();

    public const int Count = 1000;
    
    private void Start()
    {
        _enemyPool = GetComponent<EnemyPool>();
        StartCoroutine(Spawn());
        StartCoroutine(Return());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(5f);
            for (int j = 0; j < Count; j++)
            {
                var enemy = _enemyPool.Get();
                enemy.transform.position = transform.position;
                _enemies.Add(enemy);
            }
        }
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(5f);
            foreach (var enemy in _enemies)
            {
                _enemyPool.Return(enemy);
            }
            _enemies.Clear();
        }
    }
}
