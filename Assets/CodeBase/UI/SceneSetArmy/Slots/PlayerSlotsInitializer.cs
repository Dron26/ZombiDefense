using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneSetArmy.Slots
{
    [DisallowMultipleComponent]
    public class PlayerSlotsInitializer : MonoCache
    {
        public int Number => _numberGroupPosition;
        private int _numberGroupPosition;
        private List<HumanoidUI> _platoon ;
        private List<GameObject> _panels ;
        
        private int _countPanels;
        private HumanoidUI _tempCharacter;
        private ArmyInitializer _armyInitializer;

        public void Initialize(List<HumanoidUI> platoon, GameObject panel, ArmyInitializer armyInitializer)
        {
            _armyInitializer = armyInitializer;
            _armyInitializer.ClickButtonBack += DeleteAllPanel;
            _platoon = new();
            _countPanels = platoon.Count;
            
            foreach (HumanoidUI humanoid in platoon)
            {
                _platoon.Add(humanoid);
            }

            InitializePanel(panel);
        }

        public List<GameObject> GetPanels()
        {
            List<GameObject> tempPanels = new List<GameObject>();
            
            foreach (GameObject panel in _panels)
            {
                GameObject newPanel = panel;
                tempPanels.Add(newPanel);
            }

            return tempPanels;
        }

        private void InitializePanel(GameObject panel)
        {
            _panels = new();
            for (int i = 0; i < _countPanels; i++)
            {
                GameObject newPanel = Instantiate(panel, transform);
                ParametrSlot slot = newPanel.GetComponentInChildren<ParametrSlot>();
                slot.Initialize();

                _tempCharacter = _platoon[i];
                GameObject character = Instantiate(_tempCharacter.gameObject, slot.transform);
                Slider slider = newPanel.GetComponentInChildren<Slider>();
                newPanel.GetComponentInChildren<SliderUnit>().Initialize(i);
                _panels.Add(newPanel);
            }
        }
        
        public void DeleteAllPanel()
        {
            int childCount = transform.childCount;
            
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            _armyInitializer.ClickButtonBack -= DeleteAllPanel;
        }
    }
}
