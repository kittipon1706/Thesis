using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineRetarget : MonoBehaviour
{
    public CinemachineVirtualCamera myCinemachine;
    public GameObject Player;

    void Start()
    {
        myCinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        if (!Player)
        {
            Player = GameObject.FindWithTag("Player");
        }
        myCinemachine.m_Follow = Player.transform;
        myCinemachine.m_LookAt = Player.transform;
    }
}
