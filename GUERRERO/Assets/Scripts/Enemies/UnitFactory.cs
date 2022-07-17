using System.Collections.Generic;
using UnityEngine;
using static Guerrero.Data.Data;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance;
    public enum EnemiesType
    {
        Goblin,
        Dragon,
        Orc,
        Unicorn,
        Wolf,
    }

    public List<CharacterData> UnitLists;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

    }
}
