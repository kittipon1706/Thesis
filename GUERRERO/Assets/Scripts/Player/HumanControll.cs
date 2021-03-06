using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HumanControll : MonoBehaviourPun
{
     PhotonView view;
     float inputX;
     float inputZ;
     Vector3 direction;

     float directionY;
    public Transform buildPoint;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");



            if (inputX == 0 && inputZ == 0)
            {
                CharacterCore.Instance.characterData._animator.SetBool("isRunning", false);                
            }            
            else
            {
                CharacterCore.Instance.characterData._animator.SetBool("isRunning", true);
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                UiCore.Instance.MarketPanel.SetActive(true);
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                UiCore.Instance.MarketPanel.SetActive(false);
            }
            /* if (Input.GetKeyDown(KeyCode.E))
             {
                 //Debug.Log("HoloGram Render Tower");
             }
             if (Input.GetKeyUp(KeyCode.E))
             {
                 bool canmebuy = false;
                 string nametobuy = "";
                 MarketCore.Instance.BuyProcessing(out nametobuy,out canmebuy, CharacterCore.Instance.characterData, this.transform, this.gameObject.name, "SentryLevel1", MarketCore.marketType.Buy, 1, BuildingCore.BuildingType.Tower);
             }
             if (Input.GetKeyDown(KeyCode.Q))
             {
                 //Debug.Log("HoloGram Render Barricade");
             }
             if (Input.GetKeyUp(KeyCode.Q))
             {
                 bool canmebuy = false;
                 string nametobuy = "";
                 MarketCore.Instance.BuyProcessing(out nametobuy, out canmebuy, CharacterCore.Instance.characterData, this.transform, this.gameObject.name, "BarricadeLevel1", MarketCore.marketType.Buy, 1, BuildingCore.BuildingType.Barricade);
             }
             if (Input.GetKeyDown(KeyCode.F))
             {
                 //Debug.Log("HoloGram Render Barricade");
             }
             if (Input.GetKeyUp(KeyCode.F))
             {
                 bool canmebuy = false;
                 string nametobuy = "";
                 MarketCore.Instance.BuyProcessing(out nametobuy, out canmebuy, CharacterCore.Instance.characterData, this.transform, this.gameObject.name, "TrapLevel1", MarketCore.marketType.Buy, 1, BuildingCore.BuildingType.Trap);
             }*/

        }
    }
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (CharacterCore.Instance.characterData._controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    CharacterCore.Instance.TakeDamnge(CharacterCore.Instance.characterData.damage);//Don't forgot to Delete This Line
                    directionY = CharacterCore.Instance.characterData.jumpForce;
                }
                else
                {
                    directionY = 0f;
                }
            }
            else
            {
                directionY -= CharacterCore.Instance.characterData.gravity * Time.deltaTime;
            }

            direction = new Vector3(inputX * CharacterCore.Instance.characterData.moveSpeed, direction.y, inputZ * CharacterCore.Instance.characterData.moveSpeed);
            direction.y = directionY;
            CharacterCore.Instance.characterData._controller.Move(direction);

            if ((inputZ != 0 || inputX != 0))
            {
                Vector3 facing = new Vector3(direction.x, 0, direction.z);
                transform.rotation = Quaternion.LookRotation(facing);
            }
        }        
    }
}
