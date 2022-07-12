using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance;


    [System.Serializable]
    public class EnemyList
    {
        public GameObject model;
        public Avatar avatar;
        public AnimatorController animator;
    }

    public List<EnemyList> EnemyLists;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
}
