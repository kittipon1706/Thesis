using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeCtrl : MonoBehaviour
{
    [SerializeField] private int Health;
    private Action<int> OnTakeDamage = null;
    private Action OnDestroy = null;
    // Start is called before the first frame update
    void Start()
    {
        OnTakeDamage += TakeDamage;
        OnDestroy += Destroy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health<=0)
        {
            OnDestroy?.Invoke();
        }
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
        EventHandler.Instance.OnUpdateTarget?.Invoke();
        //back to pool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WeaponEN") 
        {
           //OnTakeDamage?.Invoke();
        }
    }
}
