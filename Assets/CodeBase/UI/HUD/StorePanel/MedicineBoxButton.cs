using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class MedicineBoxButton:MonoCache
    {
        [SerializeField] private Button _button;
        public Action OnSelected;

        private void OnSelectUpgrade()
        {
            OnSelected?.Invoke();
        }

        public void SwitchStateButton(bool isActive)
        {
            _button.gameObject.SetActive(isActive);
        }
    }
}