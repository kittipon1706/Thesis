using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    private Action<string,GameObject> OnDeath = null;
    [SerializeField] private float Attack_Speed = 6;
    [SerializeField] private float Move_Speed = 10;
    [SerializeField] private int ATK = 20;
    private int CurrentHP;
    private SphereCollider Weapon;
    private void Start()
    {
        Initialize(
           Model_Name: "GoblinModel",
           scale: new Vector3(10, 10, 10),
           ani: "Goblin_AniCtrl",
           avatar: "GoblinModelAvatar",
           BE_name: "Goblin_BE",
           name: "Goblin",
           hp: 100
        );
        CurrentHP = healthPoint;
        Collider.radius = 2.9f;
        Collider.height = 16.3f;
        Collider.center = new Vector3(0f, 8f, 0f) ;
        transform.localScale = new Vector3(1f, 1f, 1f);
        GetComponent<BehaviorTree>().ExternalBehavior = Resources.Load<ExternalBehavior>("Art/3D/Enemies/Goblin/Goblin_BE");
        OnDeath += ObjectPooler.Instance.BackIntoPool;
        OnDeath += Death;
        OnDeath += WaveManager.Instance.RemoveformWave;
        Goblin_Manager.Instance.OnUpdateTarget += SetTarget;
        Goblin_Manager.Instance.OnUpdateLeader += SetLeader;
        Weapon = Model.GetComponent<SphereCollider>();
        BE = GetComponent<BehaviorTree>();
        BE.SetVariableValue("Attack_Speed", Attack_Speed);
        BE.SetVariableValue("Move_Speed", Move_Speed);
        BE.SetVariableValue("Weapon", Weapon);
        SetTarget();
        SetLeader(null);
        
    }
    private void Update()
    {
        if (CurrentHP <= 0)
        {
            OnDeath?.Invoke(monsName, gameObject);
            Goblin_Manager.Instance.OnUpdateLeader?.Invoke(gameObject);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Objective")
        {
            BE.SetVariableValue("OnAttack", true);
            navmesh.SetDestination(gameObject.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Objective")
        {
            BE.SetVariableValue("OnAttack", true);
            navmesh.SetDestination(gameObject.transform.position);
        }
    }

    private void Death(string name , GameObject gameObj)
    {
        CurrentHP = healthPoint;
        BE.SetVariableValue("OnAttack", false);
        int rd = UnityEngine.Random.Range(0, 3);
        gameObject.transform.position = Goblin_Manager.Instance.spawnPoints[rd].position;
    }

    private void SetTarget()
    {
        BE.SetVariableValue("OnAttack", false);
        BE.SetVariableValue("Target", Goblin_Manager.Instance.target);
        BE.SetVariableValue("TargetTrans", Goblin_Manager.Instance.target.transform);
        BE.SetVariableValue("TargetPos", Goblin_Manager.Instance.target.transform.position);
    }

    private void SetLeader(GameObject obj)
    {
        BE.SetVariableValue("Leader", null);
        if (gameObject != Goblin_Manager.Instance.leader)
        {
            BE.SetVariableValue("Leader", Goblin_Manager.Instance.leader);
        }
    }
    private void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }

}
