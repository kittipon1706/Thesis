using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ClockSystem : MonoBehaviour , IPunObservable
{
    [SerializeField] float defaultTimeValue;
    float currentTimeValue = 0;
    [SerializeField] Text timeText;
    PhotonView view;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentTimeValue);
        }
        else
        {
            currentTimeValue = (float)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        currentTimeValue = defaultTimeValue;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (currentTimeValue > 0)
            {
                currentTimeValue -= Time.deltaTime;
            }
            else
            {
                currentTimeValue = defaultTimeValue;
            }

            DisplayTime(currentTimeValue);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToDisplay < 0)
            {
                timeToDisplay = 0;
            }

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            view.RPC("print", RpcTarget.All, minutes, seconds);
        }

    }

    [PunRPC]
    void print(float min, float sec)
    {
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
