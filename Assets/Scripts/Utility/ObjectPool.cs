using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.SingleTon;

namespace Utility
{
    public class ObjectPool : SingleMono<ObjectPool>
    {
        [SerializeField] private List<GameObject> poolObjects;
        [SerializeField] private int initCount;
        private readonly Dictionary<string, Queue<GameObject>> _poolDic = new();

        protected override void Awake()
        {
            base.Awake();
            foreach (var prefab in poolObjects)
            {
                var pool = new Queue<GameObject>();
                _poolDic.Add(prefab.name, pool);
                for (int i = 0; i < initCount; i++)
                    pool.Enqueue(Spawn(prefab));
            }
        }

        private static GameObject Spawn(GameObject prefab)
        {
            var go = Instantiate(prefab);
            go.SetActive(false);
            go.name = prefab.name;
            return go;
        }

        public GameObject Get(string prefabName)
        {
            if (!_poolDic.TryGetValue(prefabName, out var value)) return null;
            var go =  value.Count < 1 ? Spawn(poolObjects.Find(x=>x.name == prefabName)) : value.Dequeue();
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject go)
        {
            go.SetActive(false);
            if (!_poolDic.TryGetValue(go.name, out var value)) Destroy(go);
            else value.Enqueue(go);
        }
    }
}