using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Objective")
        {
            other.SendMessage("TakeDamage", 20);
            //EventHandler.Instance.OnTakeDamage?.Invoke()
        }
    }
}
