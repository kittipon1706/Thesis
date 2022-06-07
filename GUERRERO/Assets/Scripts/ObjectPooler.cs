using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size;
        public GameObject prefab;
    }

    #region Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();


    }

    public void Setup_Pool(string tag)
    {
        Pool this_pool = null;
        foreach (Pool pool in pools)
        {
            if (pool.tag == tag)
            {
                this_pool = pool;
                break;
            }
        }
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < this_pool.size; i++)
        {
            GameObject obj = Instantiate(this_pool.prefab);
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj.gameObject);
        }

        poolDictionary.Add(this_pool.tag, objectPool);
    }
    
    public GameObject SpawnFormPool (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't work");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPoolObject poolObj = objectToSpawn.GetComponent<IPoolObject>();

        if (poolObj != null)
        {
            poolObj.OnObjectSpawn();
        }

        //poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;

    }

    public GameObject BackIntoPool(string tag , GameObject objectpool , List<Component> DesComponant)
    {
        objectpool.SetActive(false);
        ObjectPooler.instance.poolDictionary[tag].Enqueue(objectpool);
        foreach (var item in DesComponant)
        {
            Destroy(item);
        }
        return objectpool;
    }

    public void AddToPool(GameObject poolObj, string tag)
    {
        Pool pool = new Pool();
        pool.prefab = poolObj;
        pool.tag = tag;
        pool.size = 5;
        pools.Add(pool);

        Setup_Pool(tag);
    }
}
