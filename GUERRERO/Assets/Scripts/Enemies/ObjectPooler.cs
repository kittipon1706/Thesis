using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size;
        public GameObject GroupObject;
        public EnemiesType type;
    }

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public enum EnemiesType
    {
        Goblin,
        Drone,
        Tank,
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private List<int> _PoolSize = new List<int>();
    public Action<string, int> OnSizeChange = null;
    public Action<string, int> OnUpdatePoolSize = null;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = new GameObject(pool.tag + (i + 1));
                obj.transform.parent = pool.GroupObject.transform;
                int rd = UnityEngine.Random.Range(0, 3);
                obj.transform.position = Goblin_Manager.Instance.spawnPoints[rd].position;
                obj.SetActive(false);
                var ScriptType = Type.GetType(pool.type.ToString());
                obj.AddComponent(ScriptType);
                objectPool.Enqueue(obj);
            }
            _PoolSize.Add(pool.size);
            poolDictionary.Add(pool.tag, objectPool);
        }

        OnSizeChange += Setup_Pool;
        OnUpdatePoolSize += UpdatePoolSize;
    }

    private void Update()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].size != _PoolSize[i] && pools[i].size > _PoolSize[i])
            {
                OnSizeChange?.Invoke(pools[i].tag , Math.Abs(pools[i].size - _PoolSize[i]));
                _PoolSize[i] = pools[i].size;
            }
        }
    }

    public void Setup_Pool(string tag,int unit)
    {
        foreach (Pool pool in pools)
        {  
            Queue<GameObject> objectPool = new Queue<GameObject>();
            if (pool.tag == tag)
            {
                objectPool = poolDictionary[pool.tag];
                var count = objectPool.Count;
                for (int i = 0; i < unit; i++)
                {
                    GameObject obj = new GameObject(pool.tag + (count + i + 1));
                    obj.transform.parent = pool.GroupObject.transform;
                    int rd = UnityEngine.Random.Range(0, 3);
                    obj.transform.position = Goblin_Manager.Instance.spawnPoints[rd].position;
                    obj.SetActive(false);
                    var ScriptType = Type.GetType(pool.type.ToString());
                    obj.AddComponent(ScriptType);
                    objectPool.Enqueue(obj);
                }
                poolDictionary[pool.tag] = objectPool;
                Debug.Log(poolDictionary[pool.tag].Count);
                break;
            }
        }
    }
    
    public GameObject SpawnFormPool (string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't work");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        return objectToSpawn;

    }

    public void BackIntoPool(string tag , GameObject objectpool)
    {
        objectpool.SetActive(false);
        poolDictionary[tag].Enqueue(objectpool);
    }

    private void UpdatePoolSize(string tag , int size)
    {
        foreach (var item in pools)
        {
            if (item.tag == tag)
            {

                item.size += size - item.size;
                break;
            }
        }
    }
}
