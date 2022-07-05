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
    
    #endregion

}
