using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetUpBuilding : MonoBehaviour
{
    public static SetUpBuilding Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    PhotonView view;
    public GameObject buildingGrp;
    private void Start()
    {
        view = GetComponent<PhotonView>();        
        view.RPC("Move", RpcTarget.All);
    }

    [PunRPC]
    public void Move()
    {
        if (buildingGrp == null)
        {
            return;
        }
        this.transform.parent = buildingGrp.transform;
    }
}
