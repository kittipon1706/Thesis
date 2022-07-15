using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCore : MonoBehaviour
{
    public static UiCore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Text healthText;
    public Text coinText;
    public GameObject MarketPanel;
    public GameObject ButtonBoxs;
    public Button productButton;
    public Text healthText_detail;
    public Text powerText_detail;

    CharacterCore.CharacterData characterData;

    private void Start()
    {
        MarketPanel.SetActive(false);
        ClearButton();
        buildMarketButton();
    }

    /*private void Update()
    {
        ClearButton();
        buildMarketButton();
    }*/

    private void FixedUpdate()
    {
        if (characterData == null)
        {
            characterData = CharacterCore.Instance.characterData;
        }
        healthText.text = "HEALTH :" + characterData.currentHealth.ToString();
        coinText.text = "COIN :" + characterData.coin.ToString();
    }

    private void buildMarketButton()
    {       
        foreach (var Target in MarketCore.Instance.buildingDataList)
        {
            if (Target.product_name != "0")
            {
                GameObject newButt = Instantiate(productButton.gameObject, ButtonBoxs.transform);
                newButt.name = Target.product_name;
                ButtItemSetting itemSetting = newButt.GetComponent<ButtItemSetting>();
                itemSetting.SetData(Target.product_name, MarketCore.marketType.Buy, Target.BuildType,Target.price, Target.maxHealth,Target.power);

                GameObject aText = newButt.transform.GetChild(0).gameObject;
                Text buttText = aText.GetComponent<Text>();
                buttText.text = Target.product_name;
            }            
        }
    }

    private void ClearButton()
    {
        foreach (Transform target in ButtonBoxs.transform)
        {
            Destroy(target.gameObject);
        }
    }

    public void ShowDetail(float health, float power)
    {
        healthText_detail.text = "HEALTH :"+ health.ToString();
        powerText_detail.text = "POWER :"+ power.ToString();
    }
}
