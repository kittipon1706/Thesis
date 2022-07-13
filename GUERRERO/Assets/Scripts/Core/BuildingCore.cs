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
            if (other.name == buildingData.ownerName)
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

        //ShowCurrentHealth();
        view.RPC("DeleteBuildingGrp", RpcTarget.All);        

        GenerateBuildingModel();
        ShowCurrentHealth();
    }

    public void DeleteBuilding_Click()
    {
        view.RPC("DeleteBuilding", RpcTarget.All);
    }

    public void GenerateBuildingModel()
    {
        if (buildingData.buildingType == BuildingType.Tower)
        {
            GenerateBuuild = PhotonNetwork.Instantiate(Sentry[buildingData.level], this.transform.position, Quaternion.identity, 0);
        }
        else if (buildingData.buildingType == BuildingType.Barricade)
        {
            GenerateBuuild  = PhotonNetwork.Instantiate(Barricade[buildingData.level], this.transform.position, Quaternion.identity, 0);
        }
        else if (buildingData.buildingType == BuildingType.Trap)
        {
            GenerateBuuild = PhotonNetwork.Instantiate(Trap[buildingData.level], this.transform.position, Quaternion.identity, 0);
        }

        Debug.Log(buildingGrp.name);
        GenerateBuuild.transform.parent = buildingGrp.transform;
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
    public void DeleteBuildingGrp()
    {
        foreach (Transform target in buildingGrp.transform)
        {
            PhotonNetwork.Destroy(target.gameObject);
        }
    }

    public void ShowCurrentHealth()
    {
        overhead_HealthText.text = buildingData.currentHealth.ToString() + "/" + buildingData.maxHealth.ToString();
    }
}
