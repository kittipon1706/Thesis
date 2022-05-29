using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviourPunCallbacks
{
    Transform target;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;
    GameObject player;

    void FixedUpdate()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(player.transform.position);
    }
    
}
