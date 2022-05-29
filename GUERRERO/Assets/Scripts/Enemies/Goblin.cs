using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    
    private void Start()
    {
        Initialize(
            speed: 4,
            Model_Name: "GoblinModel",
            position: gameObject.transform.position,
            scale: new Vector3(10, 10, 10),
            ani: "Goblin_AniCtrl",
            avatar: "GoblinModelAvatar",
            BE_name: "Goblin_BE"
        );

        gameObject.AddComponent<Goblin_Controller>();

        Collider.radius = Model.transform.localScale.x / 2;
        Collider.height = Model.transform.localScale.y * 2;
        Collider.center = new Vector3(0, Model.transform.localScale.y, Collider.radius/2) ;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
}
