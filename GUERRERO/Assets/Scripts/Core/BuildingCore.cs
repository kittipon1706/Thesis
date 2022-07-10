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
        Tower,
        Barricade,
        Trap
    }
    
    [System.Serializable]
    public class BuildingData
    {
        public string ownerID;
        public int level;
        public BuildingType buildingType;
        public int currentHealth;
        public int maxHealth;
    }    

    PhotonView view;
    public BuildingData buildingData;
    public List<string> Sentry;
    

    private void Start()
    {
        view = GetComponent<PhotonView>();        
        GameObject SentryBuild = PhotonNetwork.Instantiate(Sentry[buildingData.level], this.transform.position , Quaternion.identity, 0);
        SentryBuild.transform.parent = gameObject.transform;
    }

    void OnTriggerEnter(Collider other)
    {   
        if (other.tag == "Player")
        {
            string viewID = other.GetComponent<PhotonView>().ToString();

            if (buildingData.ownerID == viewID)
            {
                Button update_Button = UiCore.Instance.updateBuilding_Button;
                update_Button.gameObject.SetActive(true);
                string id = view.ToString();
                update_Button.onClick.AddListener(delegate { UpLevelBuilding(id); });
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        UiCore.Instance.updateBuilding_Button.gameObject.SetActive(false);
    }

    private void UpLevelBuilding(string id)
    {
        if (id != this.view.ToString())
        {
            return;
        }
        if (buildingData.level >= 3)
        {
            return;
        }

        buildingData.level++;

        foreach (Transform target in this.gameObject.transform)
        {
            PhotonNetwork.Destroy(target.gameObject);
        }

        GameObject SentryBuild = PhotonNetwork.Instantiate(Sentry[buildingData.level], this.transform.position, Quaternion.identity, 0);
        SentryBuild.transform.parent = gameObject.transform;
    }
}
