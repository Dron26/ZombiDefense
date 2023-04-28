using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.SceneSetArmy.Slots
{
    [DisallowMultipleComponent]
    public class GroupEnemySlots : MonoCache
    {
        public int Number => _numberGroupPosition;
        
        private int _numberGroupPosition;
        private List<Humanoid> _platoon = new();
        private readonly List<GameObject> _parametrPanels = new();
        private int _countPanels;

        public void Initialize(List<Humanoid> platoon, GameObject panel)
        {
            _countPanels = platoon.Count;
            foreach (Humanoid humanoid in platoon)
            {
                _platoon.Add(humanoid);
            }

            InitializePanel(panel);
        }

        private void InitializePanel(GameObject panel)
        {
            for (int i = 0; i < _countPanels; i++)
            {
                GameObject newPanel = Instantiate(panel, transform);
                ParametrSlot slot = newPanel.GetComponentInChildren<ParametrSlot>();
                GameObject called = Instantiate(_platoon[i].gameObject, slot.transform);
                slot.Initialize();
                _parametrPanels.Add(newPanel);
            }
        }
    }
}