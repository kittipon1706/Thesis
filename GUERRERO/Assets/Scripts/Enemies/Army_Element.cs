using Ingame.UI.FormationOptions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Army_Element : MonoBehaviour
{
    [SerializeField]
    private Button _Remove_Btn;
    
    [SerializeField]
    private Button _Add_Btn;
    
    [SerializeField]
    private Button _Sub_Btn;

    [SerializeField]
    private Image _Image;

    [SerializeField]
    public Text _Sizetxt; 

    [SerializeField]
    private Slider _Slider;

    public event Action<string> OnRemove;
    public event Action<string> OnAdd;
    public event Action<string> OnSub;

    public int _Cost;

    public int _Size;

    public int _Time;

    public string _Type;

    private void Awake()
    {
        _Remove_Btn.onClick.AddListener(() => OnButtonClick(OnRemove));
        _Add_Btn.onClick.AddListener(() => OnButtonClick(OnAdd));
        _Sub_Btn.onClick.AddListener(() => OnButtonClick(OnSub));
        _Slider.onValueChanged.AddListener(SetTime);
    }
    private void Start()
    {
        SetTime(_Slider.value);
        OnRemove += SetDefault;
        OnRemove += FormationOption.Instance.RemoveFormArmy;
        OnAdd += FormationOption.Instance.DirectAdd;
        OnSub += FormationOption.Instance.DirectSub;
    }

    private void OnDestroy()
    {
        _Remove_Btn.onClick.RemoveAllListeners();
        _Add_Btn.onClick.RemoveAllListeners();
        _Sub_Btn.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (_Size == 0 && _Remove_Btn.gameObject.activeSelf == true)
        {
            OnRemove?.Invoke(_Type);
            _Remove_Btn.gameObject.SetActive(false);
        }
        else if(_Size > 0 && _Remove_Btn.gameObject.activeSelf == false)
            _Remove_Btn.gameObject.SetActive(true);

    }
    public void OnButtonClick(Action<string> action) => action?.Invoke(_Type);

    public void SetTime(float value)
    {
        _Time = (int)value;
        FormationOption.Instance.SetTimeUnit(_Type, _Time);
    }

    public void SetData(int cost, Color color, string type , int size)
    {
        _Size = size;
        _Cost = cost;
        _Type = type;
        _Image.color = color;
        InGameUI_Manager.Instance.ShowText(size.ToString() , _Sizetxt);
    }

    public void SetDefault(string type)
    {
        SetData(0, Color.gray, string.Empty, 0);
        _Slider.value = 1;
    }

   
}
