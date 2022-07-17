using Ingame.UI.FormationOptions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyElement : MonoBehaviour
{
    [SerializeField]
    private Button _Button;

    [SerializeField]
    private EventTrigger _Trigger;

    [SerializeField]
    private Image _Image;  
    
    public event Action<string,int,Color> OnClick;

    private int _Cost;

    private string _Type;

    private void Awake()
    {
        _Button.onClick.AddListener(OnButtonClick);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter();});

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnPointerExit();});
        _Trigger.triggers.Add(exit);
        _Trigger.triggers.Add(entry);
        OnClick += FormationOption.Instance.SetArmy;
    }

    public void OnPointerEnter()
    {
        InGameUI_Manager.Instance.detail.GetComponent<Detail_Element>().Set_Detail(_Type);
    }  
    public void OnPointerExit()
    {
        InGameUI_Manager.Instance.detail.GetComponent<Detail_Element>().Reset_Detail();
    }

    private void OnDestroy() => _Button.onClick.RemoveAllListeners();

    public void OnButtonClick() => OnClick?.Invoke(_Type,_Cost,_Image.color);


    public void SetData(int cost, Color color, string type)
    {
        _Cost = cost;
        _Image.color = color;
        _Type = type;
    }

}
