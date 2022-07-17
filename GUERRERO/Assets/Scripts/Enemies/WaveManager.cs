using Ingame.UI.FormationOptions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Guerrero.Data.Data;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    
    public List<WaveData> Waves;
    [SerializeField] private int _NumEnemyInWave;
    [SerializeField] private int _WaveCount = 1;
    public bool ReadyToSpawn = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GManager.Instance.State == GManager.GamsState.Prepare && GManager.Instance._Time <= 0)
        {
            GManager.Instance.UpdateGameState(GManager.GamsState.Manage);
            Waves.Add(new WaveData(_WaveCount));
        }
        else if (GManager.Instance.State == GManager.GamsState.Manage && GManager.Instance._Time <= 0)
        {
            if (_WaveCount <= Waves.Count)
            {
                GManager.Instance.UpdateGameState(GManager.GamsState.Wave);
                ReadyToSpawn = true;
                if (FormationOption.Instance.ArmyUnit.Count > 0 && ReadyToSpawn)
                {
                    SetWave(FormationOption.Instance.ArmyUnit);
                    CheckPoolSize();
                }
            }
            else Debug.Log(":::Wave End:::");
            GManager.Instance._Time = 0;
        }
        else if (GManager.Instance.State == GManager.GamsState.Wave && _NumEnemyInWave <= 0)
        {
            GManager.Instance.UpdateGameState(GManager.GamsState.Manage);
            ReadyToSpawn = false;
            _WaveCount++;
            Waves.Add(new WaveData(_WaveCount));
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

    class ComparisonUnitTime : IComparer<UnitData>
    {
        public int Compare(UnitData x , UnitData y)
        {
            if (x.time == 0 && y.time == 0)
            {
                return 0;
            }

            return x.time.CompareTo(y.time);
        }
    }

    private void CheckPoolSize()
    {
        ComparisonUnitTime gg = new ComparisonUnitTime();
        Waves[_WaveCount-1].enemy.Sort(gg);
        Dictionary<string,int> sumUnit = new Dictionary<string,int>();
        for (int i = 0; i < Waves[_WaveCount-1].enemy.Count; i++)
        {
            _NumEnemyInWave += Waves[_WaveCount-1].enemy[i].size;
            if (!sumUnit.ContainsKey(Waves[_WaveCount-1].enemy[i].tag))
            {
                sumUnit.Add(Waves[_WaveCount-1].enemy[i].tag, Waves[_WaveCount-1].enemy[i].size);
            }
            else
            {
                sumUnit[Waves[_WaveCount-1].enemy[i].tag] += Waves[_WaveCount-1].enemy[i].size;
            }

            if (sumUnit[Waves[_WaveCount-1].enemy[i].tag] > ObjectPooler.Instance.poolDictionary[Waves[_WaveCount-1].enemy[i].tag].Count)
            {
                ObjectPooler.Instance.OnUpdatePoolSize?.Invoke(Waves[_WaveCount-1].enemy[i].tag, sumUnit[Waves[_WaveCount-1].enemy[i].tag]);
            }
        }

        TaskDelay(1f);

        for (int i = 0; i < Waves[_WaveCount-1].enemy.Count; i++)
        {
            StartCoroutine(SpawnEnemies(i));
            var index = Waves[_WaveCount - 1].enemy.IndexOf(Waves[_WaveCount - 1].enemy[i]);
            if (index != Waves[_WaveCount - 1].enemy.Count-1)
            {
                if ((Waves[_WaveCount - 1].enemy[i + 1].time == Waves[_WaveCount - 1].enemy[i + 1].time))
                {
                    StartCoroutine(SpawnEnemies(i + 1));
                    i++;
                }
            }
        }
    }

    private IEnumerator SpawnEnemies(int index)
    {
        yield return new WaitForSeconds(Waves[_WaveCount-1].enemy[index].time);
        for (int i = 0; i < Waves[_WaveCount-1].enemy[index].size; i++)
        {
            yield return new WaitForSeconds(1f);
            var obj = ObjectPooler.Instance.SpawnFormPool(Waves[_WaveCount-1].enemy[index].tag);
        }
    }

    public void RemoveformWave(string name,GameObject gameObject)
    {
        _NumEnemyInWave--;
    }

    private void SetWave(List<UnitData> armyDatas)
    {
        WaveData wave = new WaveData(_WaveCount);
        foreach (var item in Waves)
        {
            if (item.name == wave.name)
            {
                Waves.Remove(item);
                break;
            }
        }
        foreach (var item in armyDatas)
        {
            wave.enemy.Add(item);
        }
        Waves.Add(wave);
    }
}
