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
    [SerializeField] GameObject Player_Camera;
    [SerializeField] GameObject TPS;
    [SerializeField] GameObject TPS_Zoom;

    [SerializeField] int timeToLaunch;

    [SerializeField] Material red;
    [SerializeField] Material blue;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        //Instantiate(Player_Camera, Vector3.zero, Quaternion.identity);        

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player1tmp;
            SpwanObj(spawnPlayerTarget1, playerPrefab, ObjType.Player, out player1tmp);

            GameObject house1tmp;
            SpwanObj(spawnHouseBaseTarget1, houseBasePrefab, ObjType.BaseHouse, out house1tmp);

            string mastername = PlayerPrefs.GetString("MasterPlayername");

            view.RPC("ChangeName", RpcTarget.All, player1tmp.name,"Player",true , mastername);
            view.RPC("ChangeName", RpcTarget.All, house1tmp.name,"House",true , mastername);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            GameObject player2tmp;
            SpwanObj(spawnPlayerTarget2, playerPrefab, ObjType.Player, out player2tmp);

            GameObject house2tmp;
            SpwanObj(spawnHouseBaseTarget2, houseBasePrefab, ObjType.BaseHouse, out house2tmp);

            string minorname = PlayerPrefs.GetString("MinorPlayername");

            view.RPC("ChangeName", RpcTarget.All, player2tmp.name, "Player",false , minorname);
            view.RPC("ChangeName", RpcTarget.All, house2tmp.name, "House",false , minorname);
        }
        Instantiate(TPS, Vector3.zero, Quaternion.identity);
        Instantiate(TPS_Zoom, Vector3.zero, Quaternion.identity);
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
        myObj = OBJ;
    }

    [PunRPC]
    public void ChangeName(string objname, string objType, bool ismaster,string nametochange)
    {
        GameObject obj = GameObject.Find(objname);
        if (ismaster == true)
        {
            string newName = nametochange + "_" + objType;
            obj.name = newName;
        }
        else if (ismaster == false)
        {
            string newName = nametochange + "_" + objType;
            obj.name = newName;
        }       

    }
}
