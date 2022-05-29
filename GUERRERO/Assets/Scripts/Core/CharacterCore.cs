using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterCore : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public CharacterController _controller;
        public Animator _animator;
        public string playerName;
        public int playerLevel;

        public float moveSpeed;
        public float jumpForce;
        public float gravity;

        public float maxhealth;
        public float currentHealth;
        public float damage;
        public float armor;
        public int coin;
    }    

    //-------Singleton
    PhotonView view;
    public static CharacterCore Instance;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    //-------Singleton

    public CharacterData characterData;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            characterData.playerName = ServerCore.Instance.namePlayer;

            characterData.currentHealth = characterData.maxhealth;
            if (!characterData._controller || !characterData._animator)
            {
                GameObject tempPlayer = this.gameObject;
                characterData._controller = tempPlayer.GetComponent<CharacterController>();
                characterData._animator = tempPlayer.GetComponent<Animator>();
            }
        }
    }

    void Update()
    {
        
    }

    public void TakeDamnge(float damange)
    {
        if (view.IsMine)
        {
            characterData.currentHealth = characterData.currentHealth - damange;
        }        
    }
}
