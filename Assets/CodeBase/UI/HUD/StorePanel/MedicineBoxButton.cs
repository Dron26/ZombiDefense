using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class MedicineBoxButton:MonoCache,IBoxButton
    {
        [SerializeField] private Button _button;
        public Action OnSelected;
        
        private void Awake()
        {
            _button.onClick.AddListener(() => 
            {
                OnSelected?.Invoke();
            });
        }
        public void SwitchStateButton(bool isActive)
        {
            _button.gameObject.SetActive(isActive);
        }
    }
}