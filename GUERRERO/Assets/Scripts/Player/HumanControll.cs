using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HumanControll : MonoBehaviour
{
     PhotonView view;
     float inputX;
     float inputZ;
     Vector3 direction;

     float directionY;

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
                    CharacterCore.Instance.TakeDamnge(CharacterCore.Instance.characterData.damage);
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
