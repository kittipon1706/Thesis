using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class BuildingCore : MonoBehaviourPunCallbacks
{
    public static BuildingCore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public enum BuildingType
    {
        none,
        Tower,
        Barricade,
        Trap
    }

    [System.Serializable]
    public class BuildingData
    {
        public string buildingName;
        public string ownerName;
        public int level;
        public BuildingType buildingType;
        public float currentHealth;
        public float maxHealth;
        public float power;
    }

    PhotonView view;
    public BuildingData buildingData;
    public List<string> Sentry;
    public List<string> Barricade;
    public List<string> Trap;
    public Canvas sentryCanvas;
    public Button updateText;
    public Button destroyText;
    public GameObject buildingGrp;
    public CharacterCore.CharacterData chaDataOwner;
    public Text overhead_HealthText ;

    private GameObject GenerateBuuild;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        GenerateBuildingModel();
        sentryCanvas.gameObject.SetActive(false);
        ShowCurrentHealth();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            string otherName = other.name;

            if (buildingData.ownerName == otherName)
            {
                sentryCanvas.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        sentryCanvas.gameObject.SetActive(false);
    }

    public void UpLevelBuilding_Click()
    {
        if (buildingData.level >= 3)
        {
            Debug.Log("Level >= 3");
            return;
        }

        bool canmebuy = false;
        string nametoBuy = "";
        MarketCore.Instance.BuyProcessing(out nametoBuy,out canmebuy, chaDataOwner, this.transform, buildingData.ownerName, buildingData.buildingName, MarketCore.marketType.Update , 1, buildingData.buildingType);

        if (canmebuy == false)
        {
            return;
        }

        int i = 0;
        foreach (MarketCore.Building target in MarketCore.Instance.buildingDataList)
        {
            if (nametoBuy == target.product_name)
            {
                buildingData.level = MarketCore.Instance.buildingDataList[i].level;
                int j = i - 1;
                buildingData.currentHealth = buildingData.currentHealth + (MarketCore.Instance.buildingDataList[i].maxHealth - MarketCore.Instance.buildingDataList[j].maxHealth);
                buildingData.buildingName = MarketCore.Instance.buildingDataList[i].product_name;
                buildingData.maxHealth = MarketCore.Instance.buildingDataList[i].maxHealth;
                buildingData.power = MarketCore.Instance.buildingDataList[i].power;
                this.gameObject.name = nametoBuy;
                break;
            }
            else
            {
                i++;
            }
        }

        ShowCurrentHealth();
        foreach (Transform target in buildingGrp.transform)
        {
            PhotonNetwork.Destroy(target.gameObject);
        }

        GenerateBuildingModel();
    }

    public void DeleteBuilding_Click()
    {
        view.RPC("DeleteBuilding", RpcTarget.AllBuffered);
    }

    public void GenerateBuildingModel()
    {
        if (buildingData.buildingType == BuildingType.Tower)
        {
            GenerateBuuild = PhotonNetwork.Instantiate(Sentry[buildingData.level], this.transform.position, Quaternion.identity, 0);
            //SentryBuild.transform.parent = buildingGrp.transform;
        }
        else if (buildingData.buildingType == BuildingType.Barricade)
        {
            GenerateBuuild  = PhotonNetwork.Instantiate(Barricade[buildingData.level], this.transform.position, Quaternion.identity, 0);
            //BarricadeBuild.transform.parent = buildingGrp.transform;
        }
        else if (buildingData.buildingType == BuildingType.Trap)
        {
            GenerateBuuild = PhotonNetwork.Instantiate(Trap[buildingData.level], this.transform.position, Quaternion.identity, 0);
            //TrapBuild.transform.parent = buildingGrp.transform;
        }

        view.RPC("SetBuildingChild", RpcTarget.All);
    }


    public void TakeDanage(float damange)
    {
        if (view.IsMine)
        {            
            buildingData.currentHealth = buildingData.currentHealth - damange;
            if (buildingData.currentHealth <= 0)
            {
                view.RPC("DeleteBuilding", RpcTarget.All);
            }
            else
            {
                ShowCurrentHealth();
            }            
        }
    }

    [PunRPC]
    public void DeleteBuilding()
    {        
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void SetBuildingChild()
    {
        /* if (GenerateBuuild == null)
         {
             Debug.Log("GENERATE NULL"); 
         }
         if (buildingGrp == null)
         {
             Debug.Log("BUILDINGGRP NULL");
         }
         GenerateBuuild.transform.parent = buildingGrp.transform;*/
        GenerateBuuild.transform.SetParent(buildingGrp.transform);
    }

    public void ShowCurrentHealth()
    {
        overhead_HealthText.text = buildingData.currentHealth.ToString() + "/" + buildingData.maxHealth.ToString();
    }
}
