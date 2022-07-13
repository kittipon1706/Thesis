using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MarketCore : MonoBehaviourPun
{
    public static MarketCore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [System.Serializable]
    public class Building
    {
        public string product_name;
        public float price;
        public int level;
        public float maxHealth;
        public float power;
    }

    public enum marketType
    {
        Buy,
        Update
    }

    public List<Building> buildingDataList = new List<Building>();
    public string aview = null;
    public CharacterCore.CharacterData aplayerData = null;
    public BuildingCore.BuildingType abuildingType = BuildingCore.BuildingType.none;

    public void BuyBuilding(out bool CanBuy, CharacterCore.CharacterData playerData, string nametoBuy, float amount)
    {
        CanBuy = false;
        float price = 0;
        foreach (Building target in buildingDataList)
        {
            price = amount * target.price;
            if (target.product_name == nametoBuy && playerData.coin >= price)
            {
                CanBuy = true;
                playerData.coin = playerData.coin - price;
                break;
            }
            else
            {
                CanBuy = false;
            }
        }
        Debug.Log(CanBuy);
    }
    public void BuildSomething(Transform buildTarget, string ownerNAme, string nametoBuy, CharacterCore.CharacterData playerData, BuildingCore.BuildingType buildingType)
    {
        GameObject building = PhotonNetwork.Instantiate(("Art/3D/Building/TestTowerPrefab"), buildTarget.position, Quaternion.identity, 0);

        //building.name = nametoBuy;

        aview = ownerNAme;
        aplayerData = playerData;
        abuildingType = buildingType;
        var type = buildingType;
        PhotonView photonView1 = this.GetComponent<PhotonView>();
        photonView1.RPC("BuildProcessing", RpcTarget.All, building.name, nametoBuy, type);
    }

    [PunRPC]
    void BuildProcessing(string nameobj, string nametochange, BuildingCore.BuildingType buildingType)
    {
        GameObject G = GameObject.Find(nameobj);
        G.name = nametochange;
        G.gameObject.GetComponent<BuildingCore>().buildingData.ownerName = aview;
        G.gameObject.GetComponent<BuildingCore>().buildingData.buildingName = nametochange;
        G.gameObject.GetComponent<BuildingCore>().buildingData.buildingType = buildingType;
        G.gameObject.GetComponent<BuildingCore>().chaDataOwner = aplayerData;

        int i = 0;

        foreach (Building target in buildingDataList)
        {
            if (target.product_name == nametochange)
            {
                G.gameObject.GetComponent<BuildingCore>().buildingData.level = buildingDataList[i].level;
                G.gameObject.GetComponent<BuildingCore>().buildingData.currentHealth = buildingDataList[i].maxHealth;
                G.gameObject.GetComponent<BuildingCore>().buildingData.maxHealth = buildingDataList[i].maxHealth;
                G.gameObject.GetComponent<BuildingCore>().buildingData.power = buildingDataList[i].power;
            }
            else
            {
                i++;
            }
        }

        aview = null;
        aplayerData = null;
        abuildingType = BuildingCore.BuildingType.none;
    }

    public void BuyProcessing(out string nametobuy, out bool CanBuy, CharacterCore.CharacterData playerData, Transform buildTarget, string ownerNAme, string currentname, marketType marKetType, int amount, BuildingCore.BuildingType buildingType)
    {
        Debug.Log("asdasda");
        CanBuy = false;
        nametobuy = "";
        int i = 0;
        string nametoBuy = " ";

        if (marKetType == marketType.Update)
        {
            foreach (Building building in buildingDataList)
            {
                if (i > buildingDataList.Count)
                {
                    return;
                }
                if (currentname == building.product_name)
                {
                    i++;
                    nametoBuy = buildingDataList[i].product_name;
                    break;
                }
                else
                {
                    i++;
                }
            }
        }

        else if (marKetType == marketType.Buy)
        {
            nametoBuy = currentname;
        }

        BuyBuilding(out CanBuy, playerData, nametoBuy, amount);

        if (CanBuy == false)
        {
            return;
        }

        if (marKetType == marketType.Buy)
        {
            BuildSomething(buildTarget, ownerNAme, nametoBuy, playerData, buildingType);
        }
        else
        {
            nametobuy = nametoBuy;
            return;
        }
    }

}
