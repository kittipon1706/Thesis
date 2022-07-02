using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Manager : MonoBehaviour
{
    public static Goblin_Manager Instance;

    [SerializeField] public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> AllTarget = new List<GameObject>();
    [SerializeField] public GameObject target;
    public GameObject leader;
    public Action OnUpdateLeader;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        target = AllTarget[0];
        EventHandler.Instance.OnUpdateTarget += UpdateTarget;
        OnUpdateLeader += UpdateLeader;
    }

    private void Update()
    {
        if (transform.childCount >=2 && leader == null)
        {
            leader = transform.GetChild(0).gameObject;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPooler.instance.SpawnFormPool("Goblin");
        }
    }
    private void UpdateTarget()
    {
        target = AllTarget[1];
    }

    private void UpdateLeader()
    {
        for (int i = 0; i <= transform.childCount; i++)
        {
            var obj = transform.GetChild(i).gameObject;
            if (obj.activeSelf == true)
            {
                leader = obj.gameObject;
                break;
            }

            leader = null;
        }
    }
}
