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
        [SerializeField] private T poolObjectPrefab;
        private readonly Queue<T> _pool = new();

        protected void Awake()
        {
            _pool.Enqueue(Spawn(poolObjectPrefab));
        }

        private static T Spawn(T prefab)
        {
            var go = Instantiate(prefab);
            go.gameObject.SetActive(false);
            return go;
        }

        public T Get()
        {
            var pooledObject = _pool.Count > 0 ? _pool.Dequeue() : Spawn(poolObjectPrefab);
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void Return(T pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
            _pool.Enqueue(pooledObject);
        }
    }
}