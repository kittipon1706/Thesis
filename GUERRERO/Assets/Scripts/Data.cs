using System.Collections.Generic;
using UnityEngine;

namespace Guerrero.Data
{
    public class Data 
    {
        [System.Serializable]
        public class UnitData
        {
            public string tag;
            public int size;
            public float time;
        }

        [System.Serializable]
        public class WaveData
        {
            public string name = "Wave";
            private int count;
            public List<UnitData> enemy = new List<UnitData>();

            public WaveData(int count)
            {
                this.name = name + count;
                this.count = count;
            }
        }

        [System.Serializable]
        public class PoolData
        {
            public string tag;
            public UnitFactory.EnemiesType type;
            public int size;
            public GameObject groupObject;
        }

        [System.Serializable]
        public class CharacterData
        {
            public UnitFactory.EnemiesType type;
            public GameObject model;
            public Avatar avatar;
            public RuntimeAnimatorController animator;
            public int cost;
            public Color color;
        }
    }
}

