using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.SingleTon;

namespace Utility
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private List<T> poolObjectPrefabs;
        [SerializeField] private int initCount;
        private readonly Dictionary<string, Queue<T>> _poolDic = new();

        protected void Awake()
        {
            foreach (var prefab in poolObjectPrefabs)
            {
                var pool = new Queue<T>();
                _poolDic.Add(prefab.name, pool);
                for (int i = 0; i < initCount; i++)
                    pool.Enqueue(Spawn(prefab));
            }
        }

        private static T Spawn(T prefab)
        {
            var go = Instantiate(prefab);
            go.gameObject.SetActive(false);
            go.name = prefab.name;
            return go;
        }

        public T Get(string prefabName)
        {
            if (!_poolDic.TryGetValue(prefabName, out var value)) return null;
            var tObject =  value.Count < 1 ? Spawn(poolObjectPrefabs.Find(x=>x.name == prefabName)) : value.Dequeue();
            tObject.gameObject.SetActive(true);
            return tObject;
        }

        public T Get(string prefabName, Action onReturn)
        {
            if (!_poolDic.TryGetValue(prefabName, out var value)) return null;
            var tObject =  value.Count < 1 ? Spawn(poolObjectPrefabs.Find(x=>x.name == prefabName)) : value.Dequeue();
            tObject.gameObject.SetActive(true);
            return tObject;
        }

        public void Return(T tObject)
        {
            tObject.gameObject.SetActive(false);
            if (!_poolDic.TryGetValue(tObject.name, out var value)) Destroy(tObject);
            else value.Enqueue(tObject);
        }
    }
}