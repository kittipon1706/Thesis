using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private int _NumEnemyInWave;
    [SerializeField] private float _TimeBetweenWave;
    [SerializeField] private int _WaveCount;
    [SerializeField] private float _TimeToSpawm;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _TimeToSpawm = _TimeBetweenWave;
    }

    private void Update()
    {
        if (GManager.Instance.State == GManager.GamsState.Prepare)
        {
            _TimeToSpawm -= Time.deltaTime;
            if (_TimeToSpawm <= 0)
            {
                if (Waves.Count <= _WaveCount)
                {
                    GManager.Instance.UpdateGameState(GManager.GamsState.EnemyWave);
                    CheckPoolSize();
                }
                else Debug.Log(":::Wave End:::");

                _TimeToSpawm = 0;
            }
        }
        else if (GManager.Instance.State == GManager.GamsState.EnemyWave)
        {
            _TimeToSpawm += Time.deltaTime;
            if (_NumEnemyInWave <= 0)
            {
                GManager.Instance.UpdateGameState(GManager.GamsState.Clean);
                _TimeToSpawm = _TimeBetweenWave;
                _WaveCount++;
            }
        }
        else if (GManager.Instance.State == GManager.GamsState.Clean)
        {
            GManager.Instance.UpdateGameState(GManager.GamsState.Prepare);
        }

        AttackMonsterByClick();
    }

    private async void TaskDelay(float Delaytime)
    {
        var time = Delaytime * 1000;
        await Task.Delay((int)time);
    }

    /// <summary>
    /// For Test[Click to enemy for attack.]
    /// </summary>
    private void AttackMonsterByClick()
    {
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
    }

    private void CheckPoolSize()
    {
        Dictionary<string,int> sumUnit = new Dictionary<string,int>();
        for (int i = 0; i < Waves[_WaveCount].enemy.Count; i++)
        {
            _NumEnemyInWave += Waves[_WaveCount].enemy[i].size;
            if (!sumUnit.ContainsKey(Waves[_WaveCount].enemy[i].tag))
            {
                sumUnit.Add(Waves[_WaveCount].enemy[i].tag, Waves[_WaveCount].enemy[i].size);
            }
            else
            {
                sumUnit[Waves[_WaveCount].enemy[i].tag] += Waves[_WaveCount].enemy[i].size;
            }

            if (sumUnit[Waves[_WaveCount].enemy[i].tag] > ObjectPooler.Instance.poolDictionary[Waves[_WaveCount].enemy[i].tag].Count)
            {
                ObjectPooler.Instance.OnUpdatePoolSize?.Invoke(Waves[_WaveCount].enemy[i].tag, sumUnit[Waves[_WaveCount].enemy[i].tag]);
            }
        }

        TaskDelay(1f);

        for (int i = 0; i < Waves[_WaveCount].enemy.Count; i++)
        {
            StartCoroutine(SpawnEnemies(i));
        }
    }

    private IEnumerator SpawnEnemies(int index)
    {
        yield return new WaitForSeconds(Waves[_WaveCount].enemy[index].time);
        for (int i = 0; i < Waves[_WaveCount].enemy[index].size; i++)
        {
            yield return new WaitForSeconds(1f);
            var obj = ObjectPooler.Instance.SpawnFormPool(Waves[_WaveCount].enemy[index].tag);
        }
        Waves[_WaveCount].enemy[index].size = 0;
    }

    public void RemoveformWave(string name,GameObject gameObject)
    {
        _NumEnemyInWave--;
    }
}
