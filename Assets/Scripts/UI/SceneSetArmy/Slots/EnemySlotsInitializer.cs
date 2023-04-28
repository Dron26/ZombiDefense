using System.Collections.Generic;
using EnemiesUI.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.SceneSetArmy.Slots
{
    [DisallowMultipleComponent]
    public class EnemySlotsInitializer : MonoCache
    {
        public int Number => _numberGroupPosition;

        private int _numberGroupPosition;
        private List<Enemy> _platoon;
        private int _countPanels;
        private Enemy _tempCharacter;
        private List<GameObject> _panels;
        private ArmyInitializer _armyInitializer;
        private List<Transform> _list;
        private List<ParametrSlot> _slots;

        public void Initialize(List<Enemy> platoon, GameObject panel, ArmyInitializer armyInitializer)
        {
            _list = new List<Transform>();
            _slots = new List<ParametrSlot>();
            _armyInitializer = armyInitializer;
            _armyInitializer.ClickButtonBack += DeleteAllPanel;
            _platoon = new();
            _countPanels = platoon.Count;

            foreach (Enemy enemy in platoon)
            {
                _platoon.Add(enemy);
            }

            InitializePanel(panel);
            ClearOldPlatoon(platoon);
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
                _panels.Add(newPanel);
                _slots.Add(slot);
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

        private void ClearOldPlatoon(List<Enemy> platoon)
        {
            foreach (Enemy enemy in platoon)
            {
                Destroy(enemy.gameObject);
            }
            
        }
    }
}