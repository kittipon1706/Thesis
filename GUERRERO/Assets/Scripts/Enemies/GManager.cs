using Ingame.UI.FormationOptions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
    public GamsState State;
    public static event Action<GamsState> OnGameStateChange;
    [SerializeField] 
    public float _Time;
    [SerializeField] 
    private float _TimeBetweenWave;
    public enum GamsState
    {
        Prepare,
        Manage,
        Wave
    }
    /// <summary>
    /// mock data player
    /// </summary>
    [SerializeField]
    public int _Money;
 
    /// <summary>
    /// mock data player
    /// </summary>
    public bool CheckMoney(int cost)
    {
        if (_Money < cost)
        {
            Debug.Log("Your money not enough!!!");
            return false;
        }
        else return true;
    }
    /// <summary>
    /// mock data player
    /// </summary>
    public void SetMoney(int value)
    {
        _Money = value;
        InGameUI_Manager.Instance.ShowText(_Money.ToString(), InGameUI_Manager.Instance._Money_txt);
    }

    public IEnumerator  WarnMoney()
    {
        Color color = InGameUI_Manager.Instance._Money_txt.color;
        InGameUI_Manager.Instance._Money_txt.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        InGameUI_Manager.Instance._Money_txt.color = color;
         
    }


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
        OnGameStateChange += FormationOption.Instance.GameStateChange;
        UpdateGameState(GamsState.Prepare);
        SetMoney(_Money);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && State == GamsState.Manage)
        {
            FormationOption.Instance.gameObject.SetActive(!FormationOption.Instance.gameObject.activeSelf);
        }

        if (State == GamsState.Wave)
        {
            _Time += Time.deltaTime;
        }
        else _Time -= Time.deltaTime;
    }

    public void UpdateGameState(GamsState newState)
    {
        State = newState;

        switch (newState)
        {
            case GamsState.Prepare:
                break;
            case GamsState.Manage:
                _Time = _TimeBetweenWave;
                break;
            case GamsState.Wave:
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }


}
