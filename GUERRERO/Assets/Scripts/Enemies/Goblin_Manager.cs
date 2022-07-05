using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Manager : MonoBehaviour
{
    public static Goblin_Manager Instance;

    [SerializeField] public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> allTarget = new List<GameObject>();
    [SerializeField] public GameObject target;
    public GameObject leader;
    public Action<GameObject> OnUpdateLeader;
    public Action OnUpdateTarget;
    private void Awake()
    {
        Instance = this;
        target = allTarget[0];
        OnUpdateTarget += UpdateTarget;
        OnUpdateLeader += UpdateLeader;
    }

    private void Update()
    {
        if (transform.childCount >=2 && leader == null)
        {
            leader = transform.GetChild(0).gameObject;
        }
    }
    private void UpdateTarget()
    {
        target = allTarget[1];
    }

    private void UpdateLeader(GameObject obj)
    {
        if (obj == leader)
        {
            for (int i = 0; i <= transform.childCount; i++)
            {
                obj = transform.GetChild(i).gameObject;
                if (obj.activeSelf == true)
                {
                    leader = obj.gameObject;
                    break;
                }

                leader = null;
            }
        }
    }
}
