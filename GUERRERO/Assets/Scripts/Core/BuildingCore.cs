using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BuildingCore : MonoBehaviour
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
        public PhotonView ownerID;
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
            string viewID = other.GetComponent<PhotonView>().ToString();

            if (buildingData.ownerID.ToString() == viewID)
            {
                /*Button update_Button = UiCore.Instance.updateBuilding_Button;
                update_Button.gameObject.SetActive(true);
                string id = view.ToString();
                update_Button.onClick.AddListener(delegate { UpLevelBuilding(id); });*/
                sentryCanvas.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //UiCore.Instance.updateBuilding_Button.gameObject.SetActive(false);
        sentryCanvas.gameObject.SetActive(false);
    }

    public void UpLevelBuilding()
    {
        if (buildingData.level >= 3)
        {
            Debug.Log("Level >= 3");
            return;
        }

        bool canmebuy = false;
        string nametoBuy = "";
        MarketCore.Instance.BuyProcessing(out nametoBuy,out canmebuy, chaDataOwner, this.transform, buildingData.ownerID, buildingData.buildingName, MarketCore.marketType.Update , 1, buildingData.buildingType);
        
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

    public void GenerateBuildingModel()
    {
        if (buildingData.buildingType == BuildingType.Tower)
        {
            GameObject SentryBuild = PhotonNetwork.Instantiate(Sentry[buildingData.level], this.transform.position, Quaternion.identity, 0);
            SentryBuild.transform.parent = buildingGrp.transform;
        }
        else if (buildingData.buildingType == BuildingType.Barricade)
        {
            GameObject BarricadeBuild = PhotonNetwork.Instantiate(Barricade[buildingData.level], this.transform.position, Quaternion.identity, 0);
            BarricadeBuild.transform.parent = buildingGrp.transform;
        }
        else if (buildingData.buildingType == BuildingType.Trap)
        {
            GameObject BarricadeBuild = PhotonNetwork.Instantiate(Trap[buildingData.level], this.transform.position, Quaternion.identity, 0);
            BarricadeBuild.transform.parent = buildingGrp.transform;
        }
    }

    public void TakeDanage(float damange)
    {
        if (view.IsMine)
        {            
            buildingData.currentHealth = buildingData.currentHealth - damange;
            if (buildingData.currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                ShowCurrentHealth();
            }            
        }
    }

    public void DeleteBuilding()
    {
        Destroy(this.gameObject);
    }

    public void ShowCurrentHealth()
    {
        overhead_HealthText.text = buildingData.currentHealth.ToString() + "/" + buildingData.maxHealth.ToString();
    }
}
