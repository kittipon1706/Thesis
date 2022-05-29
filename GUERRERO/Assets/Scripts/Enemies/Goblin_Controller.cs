using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
public class Goblin_Controller : MonoBehaviour
{
    private Behavior _BE;
    [SerializeField] private GameObject Player;
    [SerializeField] private float Attack_Speed = 6;
    [SerializeField] private float Move_Speed = 10;
    private void Start()
    {

        Player = GameObject.FindGameObjectWithTag("Player");

        _BE = GetComponent<BehaviorTree>();

        _BE.SetVariableValue("Target", Player);
        _BE.SetVariableValue("Attack_Speed", Attack_Speed);
        _BE.SetVariableValue("Move_Speed", Move_Speed);

        var direction = (Player.transform.position - gameObject.transform.position).normalized;
        gameObject.transform.forward = direction;
    }

    private void Update()
    {
    }

}
