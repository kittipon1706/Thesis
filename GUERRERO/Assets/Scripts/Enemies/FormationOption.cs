using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Guerrero.Data.Data;

namespace Ingame.UI.FormationOptions
{
    public class FormationOption : MonoBehaviour
    {
        public static FormationOption Instance;
        [SerializeField]
        private int MaxSlote;
        [SerializeField]
        public Transform _UnitContainer;  
        [SerializeField]
        public Transform _ArmyContainer;
        [SerializeField]
        private Button _Confirm_btn;
        public List<UnitData> ArmyUnit = new List<UnitData>();
        public List<Army_Element> ArmyElements = new List<Army_Element>();
        public bool AllButton;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);

            _Confirm_btn.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy() => _Confirm_btn.onClick.RemoveAllListeners();

        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                Army_Element newElement = GameObject.Instantiate(InGameUI_Manager.Instance.armyPrototype, _ArmyContainer).GetComponent<Army_Element>();
                newElement.SetData(0, Color.gray, string.Empty, 0);
                ArmyElements.Add(newElement);
            }
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (ArmyUnit.Count > 0 && _Confirm_btn.interactable == false)
            {
                _Confirm_btn.interactable = true;
            }
            else if (ArmyUnit.Count == 0 && _Confirm_btn.interactable == true)
            {
                _Confirm_btn.interactable = false;
            }

            if (GManager.Instance.State != GManager.GamsState.Manage && gameObject.activeSelf == true)
            {
                gameObject.SetActive(false);
            }
        }
        private void All_ButtonSetActive(bool value)
        {
            foreach (var item in ArmyElements)
            {
                var button = item.gameObject.GetComponent<Slider>();
                if (button != null)
                {
                    button.interactable = value;
                }
            } 
            
            foreach (Transform item in _UnitContainer)
            {
                var button = item.GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = value;
                }
            }
            AllButton = !value;
        }

        private void OnButtonClick()
        {
            WaveManager.Instance.ReadyToSpawn = !WaveManager.Instance.ReadyToSpawn;
            All_ButtonSetActive(!WaveManager.Instance.ReadyToSpawn);
        }

        public void SetUnit(List<CharacterData> elementDatas, GameObject prototype)
        {
            foreach (var data in elementDatas)
            {
                    EnemyElement newElement = GameObject.Instantiate(prototype, _UnitContainer).GetComponent<EnemyElement>();
                    newElement.SetData(data.cost, data.color, data.type.ToString());
            }
        } 

        public void SetTimeUnit(string type , int time)
        {
            foreach (var item in ArmyUnit)
            {
                if (item.tag == type)
                {
                    item.time = time;
                    return;
                }
            }
        }
        public void SetArmy(string type, int cost , Color color)
        {
            if (GManager.Instance.CheckMoney(cost) == false)
            {
                StartCoroutine(GManager.Instance.WarnMoney());
                return;
            }
            else GManager.Instance.SetMoney(GManager.Instance._Money - cost);

            if (ArmyUnit.Count > 0)
            {
                foreach (var item in ArmyUnit)
                {
                    if (item.tag == type)
                    {
                        var index = ArmyUnit.IndexOf(item);
                        item.size++;
                        ArmyElements[index].SetData(cost, color, type, item.size);
                        item.time = ArmyElements[index]._Time;
                        return;
                    }
                }
            }

            if (ArmyUnit.Count < MaxSlote)
            {
                UnitData unit = new UnitData();
                unit.tag = type;
                unit.size = 1;

                foreach (var item in ArmyElements)
                {
                    if (item._Type == string.Empty)
                    {
                        item.SetData(cost, color, type, 1);
                        unit.time = item._Time;
                        ArmyUnit.Add(unit);
                        return;
                    }
                }
            }
        }

        public void RemoveFormArmy(string type)
        {
            foreach (var item in ArmyUnit)
            {
                if (item.tag == type)
                {
                    var index = ArmyUnit.IndexOf(item);
                    ArmyUnit.RemoveAt(index);
                    return;
                }
            }
        }

        public void DirectAdd(string type)
        {
            foreach (var item in ArmyUnit)
            {
                if (item.tag == type)
                {
                    var index = ArmyUnit.IndexOf(item);
                    item.size++;
                    ArmyElements[index]._Size = item.size;
                    InGameUI_Manager.Instance.ShowText(item.size.ToString(), ArmyElements[index]._Sizetxt);
                    if (GManager.Instance.CheckMoney(ArmyElements[index]._Cost))
                    {
                        GManager.Instance.SetMoney(GManager.Instance._Money - ArmyElements[index]._Cost);
                    }
                    else StartCoroutine(GManager.Instance.WarnMoney());
                    return;
                }
            }
        }

        public void DirectSub(string type)
        {
            foreach (var item in ArmyUnit)
            {
                if (item.tag == type)
                {
                    var index = ArmyUnit.IndexOf(item);
                    item.size--;
                    ArmyElements[index]._Size = item.size;
                    InGameUI_Manager.Instance.ShowText(item.size.ToString(), ArmyElements[index]._Sizetxt);
                    GManager.Instance.SetMoney(GManager.Instance._Money + ArmyElements[index]._Cost);
                    return;
                }
            }
        }

        private void SetDefault()
        {
            ArmyUnit.Clear();
            foreach (var item in ArmyElements)
            {
                item.SetDefault(string.Empty);
            }
        }

        public void GameStateChange(GManager.GamsState gamsState)
        {
            if (gamsState == GManager.GamsState.Manage)
            {
                SetDefault();
                gameObject.SetActive(true);
            }
            else return;
        }
    }
}
