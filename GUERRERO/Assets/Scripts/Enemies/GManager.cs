using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance;

    public GamsState State;

    public static event Action<GamsState> OnGameStateChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GamsState.Prepare);    
    }

    public void UpdateGameState(GamsState newState)
    {
        State = newState;

        switch (newState)
        {
            case GamsState.Prepare:
                break;
            case GamsState.EnemyWave:
                break;
            case GamsState.Clean:
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    public enum GamsState
    {
        Prepare,
        EnemyWave,
        Clean
    }
}
