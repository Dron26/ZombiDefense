using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class WorkPointUpgradePanel:MonoCache
    {
        private Button _button;
        public Action OnSelectedButton; 
        public void Initialize()
        {
            _button=GetComponent<Button>();
            _button.gameObject.SetActive(true);
            _button.onClick.AddListener(OnSelectUpgrade);
        }
        
        private void OnSelectUpgrade()
        {
            OnSelectedButton?.Invoke();
        }
        
        public void SwitchStateButton(bool isActive)
        {
            _button.gameObject.SetActive(isActive);
        }
    }
}