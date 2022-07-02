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
    #endregion

    public Action OnUpdateTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.SendMessage("TakeDamage", 50);
                }
            }
        }
    }
}
