using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayCore : MonoBehaviour
{
    public static PlayCore Instance;
    PhotonView view;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public enum GameStatus
    {
        Start,
        WaveLaunch,
        End
    }

    public enum ObjType
    {
        Player,
        BaseHouse
    }

    public GameStatus gameStatus;
    // public ObjType objType;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject houseBasePrefab;

    [SerializeField] GameObject spawnPlayerTarget1;
    [SerializeField] GameObject spawnPlayerTarget2;
    [SerializeField] GameObject spawnHouseBaseTarget1;
    [SerializeField] GameObject spawnHouseBaseTarget2;

    [SerializeField] int timeToLaunch;

    [SerializeField] Material red;
    [SerializeField] Material blue;

    //Debuger Text
    //[SerializeField] Text DebugerText;


    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player1tmp;
            SpwanObj(spawnPlayerTarget1, playerPrefab, ObjType.Player, out player1tmp);

            GameObject house1tmp;
            SpwanObj(spawnHouseBaseTarget1, houseBasePrefab, ObjType.BaseHouse, out house1tmp);

            view.RPC("ChangeTexter", RpcTarget.All, player1tmp.name, 0, 0);
            view.RPC("ChangeTexter", RpcTarget.All, house1tmp.name, 0, 0);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            GameObject player1tmp;
            SpwanObj(spawnPlayerTarget2, playerPrefab, ObjType.Player, out player1tmp);

            GameObject house1tmp;
            SpwanObj(spawnHouseBaseTarget2, houseBasePrefab, ObjType.BaseHouse, out house1tmp);

            view.RPC("ChangeTexter", RpcTarget.All, player1tmp.name, 0, 1);

            view.RPC("ChangeTexter", RpcTarget.All, house1tmp.name, 0, 1);
        }

    }

    [PunRPC]
    public void ChangeTexter(string objname, int index, int color)
    {
        /* GameObject obj = GameObject.Find(objname);
         GameObject myObj = obj.transform.GetChild(index).gameObject;
         if (color == 0)
         {
             myObj.GetComponent<Renderer>().material = red;
         }
         else if (color == 1)
         {
             myObj.GetComponent<Renderer>().material = blue;
         }*/

    }

    public void SpwanObj(GameObject target, GameObject ObjPrefab, ObjType type, out GameObject myObj)
    {
        GameObject OBJ = PhotonNetwork.Instantiate(ObjPrefab.name, target.transform.position, Quaternion.identity);
        //view.RPC("ChangeName", RpcTarget.All, OBJ.name, type.ToString());
        myObj = OBJ;
    }

    [PunRPC]
    public void ChangeName(string objname, string objType)
    {
        GameObject obj = GameObject.Find(objname);
       // DebugerText.text = ServerCore.Instance.namePlayer;
        string newName = ServerCore.Instance.namePlayer + "_" + objType;
        obj.name = newName;

    }
}
