using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PhotonView view;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    //[SerializeField] Camera playcam;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction marketAction;

    private bool onMarketPanel = false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["MOVE"];
        jumpAction = playerInput.actions["JUMP"];
        shootAction = playerInput.actions["ATTACK"];
        marketAction = playerInput.actions["MARKET"];                
    }

    private void OnEnable()
    {
        if (view.IsMine)
        {
            shootAction.performed += _ => AttackAction();
           /* marketAction.performed += _ => UiCore.Instance.MarketPanel.SetActive(true);
            marketAction.performed += _ => onMarketPanel = true;*/
        }        
    }

    private void OnDisable()
    {
        if (view.IsMine)
        {
            shootAction.performed -= _ => AttackAction();
          /*  marketAction.canceled += _ => UiCore.Instance.MarketPanel.SetActive(false);
            marketAction.canceled += _ => onMarketPanel = false;*/
        }        
    }

    void Update()
    {
        if (view.IsMine)
        {

            if (onMarketPanel == false)
            {
               // Cursor.lockState = CursorLockMode.Locked;

                groundedPlayer = controller.isGrounded;
                if (groundedPlayer && playerVelocity.y < 0)
                {
                    playerVelocity.y = 0f;
                }

                Vector2 input = moveAction.ReadValue<Vector2>();
                Vector3 move = new Vector3(input.x, 0, input.y);
                move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
                move.y = 0f;
                controller.Move(move * Time.deltaTime * CharacterCore.Instance.characterData.moveSpeed);

                // Changes the height position of the player..
                if (jumpAction.triggered && groundedPlayer)
                {
                    playerVelocity.y += Mathf.Sqrt(CharacterCore.Instance.characterData.jumpForce * -3.0f * CharacterCore.Instance.characterData.gravity);
                }

                playerVelocity.y += CharacterCore.Instance.characterData.gravity * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);

                //Rotate towards Camera Direction.
                Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, CharacterCore.Instance.characterData.rotationSpeed * Time.deltaTime);

                /* RaycastHit hit;
                 if (Physics.Raycast(Camera.main.transform.position, cameraTransform.forward, out hit, Mathf.Infinity))
                 {
                     CharacterCore.Instance.buildPoint = hit.transform;
                 }*/

            }
            else
            {
                //Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
               // Cursor.lockState = CursorLockMode.None;
               // Cursor.visible = true;
                UiCore.Instance.MarketPanel.SetActive(true);                
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
               // Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                UiCore.Instance.MarketPanel.SetActive(false);                
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Normal)
                {
                    CharacterCore.Instance.characterData.playermode = CharacterCore.PlayerMode.Update;
                }
                else
                {
                    CharacterCore.Instance.characterData.playermode = CharacterCore.PlayerMode.Normal;
                }
                Debug.Log(CharacterCore.Instance.characterData.playermode);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Normal)
                {
                    CharacterCore.Instance.characterData.playermode = CharacterCore.PlayerMode.Destroy;
                }
                else
                {
                    CharacterCore.Instance.characterData.playermode = CharacterCore.PlayerMode.Normal;
                }
                Debug.Log(CharacterCore.Instance.characterData.playermode);
            }
        }

        if (onMarketPanel == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

   /* void OnTriggerStay(Collider other)
    {
        if (other.tag == "Building")
        {
            var objcomp = other.GetComponent<BuildingCore>();
            if ((objcomp.buildingData.ownerName == this.name) && (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Update) && (Input.GetMouseButtonDown(0)))
            {
                Debug.Log("CLiCK UpDate");
                BuildingCore.Instance.UpLevelBuilding_Click();
            }
            else if ((objcomp.buildingData.ownerName == this.name) && (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Destroy) && (Input.GetMouseButtonDown(0)))
            {
                Debug.Log("CLiCK DElete");
                BuildingCore.Instance.DeleteBuilding();
            }
        }
    }*/

    private void AttackAction()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.name);
            if ((hit.transform.tag == "Building") && (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Update))
            {
                var objcomp = hit.transform.GetComponent<BuildingCore>();
                objcomp.UpLevelBuilding_Click();
            }
            else if ((hit.transform.tag == "Building") && (CharacterCore.Instance.characterData.playermode == CharacterCore.PlayerMode.Destroy))
            {
                var objcomp = hit.transform.GetComponent<BuildingCore>();
                objcomp.DeleteBuilding();
            }
        }
    }
}
