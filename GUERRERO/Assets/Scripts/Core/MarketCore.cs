using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MarketCore : MonoBehaviour
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
        public int maxHealth;
        public float power;
    }

    public enum marketType
    {
        Buy,
        Update
    }

    public List<Building> buildingDataList = new List<Building>();

    public void BuyBuilding( out bool CanBuy, CharacterCore.CharacterData playerData, string nametoBuy ,float amount)
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
        
    }

    public void BuildSomething(Transform buildTarget, PhotonView view, string nametoBuy, CharacterCore.CharacterData playerData)
    {
        GameObject building = PhotonNetwork.Instantiate(("Art/3D/Building/TestTowerPrefab"), buildTarget.position, Quaternion.identity, 0);
        building.name = nametoBuy;
        building.GetComponent<BuildingCore>().buildingData.ownerID = view;
        building.GetComponent<BuildingCore>().buildingData.buildingName = nametoBuy;
        building.GetComponent<BuildingCore>().chaDataOwner = playerData;

        int i = 0;
        foreach (Building target in buildingDataList)
        {
            if (target.product_name == nametoBuy)
            {
                building.GetComponent<BuildingCore>().buildingData.level = buildingDataList[i].level;
                building.GetComponent<BuildingCore>().buildingData.maxHealth = buildingDataList[i].maxHealth;
                building.GetComponent<BuildingCore>().buildingData.power = buildingDataList[i].power;
            }
            else
            {
                i++;
            }
        }
    }

    public void BuyProcessing( out string nametobuy,out bool CanBuy, CharacterCore.CharacterData playerData, Transform buildTarget, PhotonView view, string currentname, marketType marKetType, int amount)
    {
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
        else if(marKetType == marketType.Buy)
        {
            nametoBuy = currentname;
        }

        BuyBuilding(out CanBuy, playerData, nametoBuy,amount);
        if (CanBuy == false)
        {            
            return;
        }

        if (marKetType == marketType.Buy)
        {
            BuildSomething(buildTarget, view, nametoBuy, playerData);
        }
        else
        {
            nametobuy = nametoBuy;
            return;
        }
    }
    
}
