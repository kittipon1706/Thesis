using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtItemSetting : MonoBehaviour
{
    public string Buttname;
    public MarketCore.marketType ButtmarketType;
    public BuildingCore.BuildingType ButtBuildingType;

    public void SetData(string NameSetting, MarketCore.marketType marketTypeSetting, BuildingCore.BuildingType BuildingTypeSetting)
    {
        Buttname = NameSetting;
        ButtmarketType = marketTypeSetting;
        ButtBuildingType = BuildingTypeSetting;
    }

    public void Click()
    {
        bool canmebuy = false;
        string nametobuy = "";
        MarketCore.Instance.BuyProcessing(out nametobuy, out canmebuy, CharacterCore.Instance.characterData, CharacterCore.Instance.buildPoint, CharacterCore.Instance.name, Buttname, ButtmarketType, 1, ButtBuildingType);
    }
}
