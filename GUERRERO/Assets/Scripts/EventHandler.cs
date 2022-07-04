using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #region ObjectPooler
    public Action<string, int> OnSizeChange = null;
    public Action<string, int> OnUpdatePoolSize = null;
    #endregion

    public Action OnUpdateTarget;
}
