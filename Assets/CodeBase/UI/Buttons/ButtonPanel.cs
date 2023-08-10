using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ButtonPanel: MonoCache
    {
        [SerializeField] private Button _menu;
        [SerializeField] private GameObject _rightPanel;
        [SerializeField] private GameObject _leftPanel;

        private bool isActive=true;

        private void Start()
        {
              InitializeButton();
        }

        private void InitializeButton()
        {
        }

        public void SwitchPanelState()
        {
            isActive=!isActive;
            _rightPanel.SetActive(isActive);
            _leftPanel.SetActive(isActive);
        }
        
    }
}