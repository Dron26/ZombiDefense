using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeWindow:MonoCache
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _panel;
        
        protected override  void OnEnabled()
        {
            _backButton.onClick.AddListener(SwitchState);
        }

        protected override void OnDisabled()
        {
            _backButton.onClick.RemoveListener(SwitchState);
        }
        public void SwitchState()
        {
            _panel.SetActive(!_panel.activeInHierarchy);
        }
    }
}