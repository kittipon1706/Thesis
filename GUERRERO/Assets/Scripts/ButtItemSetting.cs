using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtItemSetting : MonoBehaviour
{
    public string Buttname;
    public MarketCore.marketType ButtmarketType;
    public BuildingCore.BuildingType ButtBuildingType;
    public Text priceText;

    float health;
    float power;

    public void SetData(string NameSetting, MarketCore.marketType marketTypeSetting, BuildingCore.BuildingType BuildingTypeSetting,float price,float ahealth,float apower)
    {
        Buttname = NameSetting;
        ButtmarketType = marketTypeSetting;
        ButtBuildingType = BuildingTypeSetting;
        priceText.text = "$"+ price.ToString();

        health = ahealth;
        power = apower;
    }

    public void Click()
    {
        bool canmebuy = false;
        string nametobuy = "";
        MarketCore.Instance.BuyProcessing(out nametobuy, out canmebuy, CharacterCore.Instance.characterData, CharacterCore.Instance.buildPoint, CharacterCore.Instance.name, Buttname, ButtmarketType, 1, ButtBuildingType);
    }

    public void HoverDetail()
    {
        UiCore.Instance.ShowDetail(health, power);
    }

    public void HoverExit()
    {
        UiCore.Instance.ShowDetail(0, 0);
    }
}
