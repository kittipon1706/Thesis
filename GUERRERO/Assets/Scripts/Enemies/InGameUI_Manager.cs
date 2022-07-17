using Ingame.UI;
using Ingame.UI.FormationOptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI_Manager : MonoBehaviour
{
    public static InGameUI_Manager Instance;

    [SerializeField]
    private GameObject _UnitPrototype;   
    
    [SerializeField]
    public GameObject detail;  
    
    [SerializeField]
    public Text _Time_txt;

    [SerializeField]
    public Text _Money_txt;

    [SerializeField]
    public GameObject armyPrototype;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _Time_txt.text = "(" + GManager.Instance.State.ToString() + ") : " + (int)GManager.Instance._Time;
    }
    public void Initialize()
    {
        FormationOption.Instance.SetUnit(UnitFactory.Instance.UnitLists, _UnitPrototype);
    }

    public void ShowText(string text, Text textComp)
    {
        MainThreadDispatcher.Instance().Enqueue(() => textComp.text = text);
    }
}

