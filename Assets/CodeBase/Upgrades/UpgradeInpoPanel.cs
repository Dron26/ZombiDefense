using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeInpoPanel:MonoCache
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _applyButton;
        [SerializeField] private GameObject _panel;
        
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;
        private Upgrade _upgrade;
        public Action OnApplyClicked;

        protected override  void OnEnabled()
        {
            _backButton.onClick.AddListener(()=>SwitchState(false));
            _applyButton.onClick.AddListener(OnClickApply);
        }

        private void OnClickApply()
        {
            OnApplyClicked?.Invoke();
            SwitchState(false);
        }

        protected override void OnDisabled()
        {
            _backButton.onClick.RemoveListener(()=>SwitchState(false));
            _applyButton.onClick.RemoveListener(OnClickApply);
        }
        public void SwitchState(bool isActive)
        {
            if (!isActive)
            {
                Reset();
            }
            _panel.SetActive(!_panel.activeInHierarchy);
        }

        private void Reset()
        {
            _upgrade = null;
            _icon.sprite = null;
            _name.text  = "";
            _description.text = "";
            _price.text ="";
        }

        public void Initialize(Upgrade upgrade)
        {
            _upgrade = upgrade;
            _icon.sprite = _upgrade.Icon;
            _name.text  = _upgrade.Name;
            _description.text = _upgrade.Description;
            _price.text ="$ "+ _upgrade.Cost;
        }
    }
}