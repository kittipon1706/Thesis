using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size;
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
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                Enemy<Goblin> GoblinClone = new Enemy<Goblin>("GoblinClone");
                GoblinClone.GameObject.SetActive(false);
                objectPool.Enqueue(GoblinClone.GameObject);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
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
}
