using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int prioritryBoostAmount = 10;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private float Speed;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        GameObject Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
        aimAction = playerInput.actions["AIM"];
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            aimAction.performed += _ => StartAim();
            aimAction.canceled += _ => CancelAim();
        }        
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            aimAction.performed -= _ => StartAim();
            aimAction.canceled -= _ => CancelAim();
        }        
    }

    private void StartAim()
    {
        virtualCamera.Priority += prioritryBoostAmount;
        Speed = CharacterCore.Instance.characterData.moveSpeed;
        CharacterCore.Instance.characterData.moveSpeed = 0;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= prioritryBoostAmount;
        CharacterCore.Instance.characterData.moveSpeed = Speed;
    }
}
