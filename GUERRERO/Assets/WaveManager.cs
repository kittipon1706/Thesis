using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [System.Serializable]
    public class Wave
    {
        public List<EnemyUnit> enemy = new List<EnemyUnit>();
    }
    
    [System.Serializable]
    public class EnemyUnit
    {
        public string tag;
        public int size;
        public float time;
    }

    public List<Wave> Waves;
    [SerializeField] private int NumEnemyInWave;
    [SerializeField] private float TimeBetweenWave;
    [SerializeField] private int WaveCount;
    [SerializeField] private bool WaveStart;
    [SerializeField] private bool WaveCounting;
    [SerializeField] private float GameTime;
    [SerializeField] private float Maxtime;
    [SerializeField] private float TimeInWave;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameTime = TimeBetweenWave;
    }

    private void Update()
    {
        if (WaveCounting)
        {
            GameTime -= Time.deltaTime;
        }

        if (GameTime <= 0)
        {
            WaveCounting = false;
            WaveStart = true;
            GameTime = TimeBetweenWave;
        }

        if (WaveStart)
        {
            CheckPoolSize();
            StartCoroutine(SpawnEnemy());
        }

        if (!WaveStart && TimeInWave < Maxtime && !WaveCounting)
        {
            TimeInWave += Time.deltaTime;
        }

        if (!WaveStart && TimeInWave >= Maxtime && NumEnemyInWave <= 0)
        {
            WaveCounting = true;
            TimeInWave = 0;
            Maxtime = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.SendMessage("TakeDamage", 50);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckPoolSize();
            //SpawnEnemy();
        }
    }

    private void CheckPoolSize()
    {
        Dictionary<string,int> sumUnit = new Dictionary<string,int>();
        for (int i = 0; i < Waves[WaveCount].enemy.Count; i++)
        {
            Maxtime += Waves[WaveCount].enemy[i].time;
            NumEnemyInWave += Waves[WaveCount].enemy[i].size;
            if (!sumUnit.ContainsKey(Waves[WaveCount].enemy[i].tag))
            {
                sumUnit.Add(Waves[WaveCount].enemy[i].tag, Waves[WaveCount].enemy[i].size);
            }
            else
            {
                sumUnit[Waves[WaveCount].enemy[i].tag] += Waves[WaveCount].enemy[i].size;
            }

            if (sumUnit[Waves[WaveCount].enemy[i].tag] > ObjectPooler.instance.poolDictionary[Waves[WaveCount].enemy[i].tag].Count)
            {
                EventHandler.Instance.OnUpdatePoolSize?.Invoke(Waves[WaveCount].enemy[i].tag, sumUnit[Waves[WaveCount].enemy[i].tag]);
            }
        }
    }

    public IEnumerator SpawnEnemy() {

        WaveStart = false;

        for (int i = 0; i < Waves[WaveCount].enemy.Count ; i++)
        {
            yield return new WaitForSeconds(Waves[WaveCount].enemy[i].time);
            for (int j = 0; j < Waves[WaveCount].enemy[i].size ; j++)
            {
                var obj = ObjectPooler.instance.SpawnFormPool(Waves[WaveCount].enemy[i].tag);
            }
            Waves[WaveCount].enemy[i].size = 0;
        }

        WaveCount++;

    }

    public void RemoveformWave(string name,GameObject gameObject)
    {
        NumEnemyInWave--;
    }
}
