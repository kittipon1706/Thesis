using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Contrainer : MonoBehaviour
{
    public static Enemy_Contrainer instance = null;
    [SerializeField] public Goblin _Goblin;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

   
}
