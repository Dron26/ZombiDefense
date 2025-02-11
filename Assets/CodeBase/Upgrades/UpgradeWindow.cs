using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeWindow:MonoCache
    {
        [SerializeField] private Button _backButton;
        
        private void OnEnable()
        {
            _backButton.onClick.AddListener(SwitchState);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(SwitchState);
        }
        public void SwitchState()
        {
            gameObject.SetActive(!isActiveAndEnabled);
        }
    }
}