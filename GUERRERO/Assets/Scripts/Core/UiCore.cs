using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCore : MonoBehaviour
{
    public static UiCore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Text healthText;
    public Text coinText;
    public Button updateBuilding_Button;

    CharacterCore.CharacterData characterData;

    private void Start()
    {
        updateBuilding_Button.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (characterData == null)
        {
            characterData = CharacterCore.Instance.characterData;
        }
        healthText.text = "HEALTH :" + characterData.currentHealth.ToString();
        coinText.text = "COIN :" + characterData.coin.ToString();
    }
}
