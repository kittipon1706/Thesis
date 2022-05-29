using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private void Awake()
    {
        Enemy<Goblin> GoblinClone = new Enemy<Goblin>("GoblinClone");
       
    }
}
