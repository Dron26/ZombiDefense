using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradePanel:MonoCache
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _applyButton;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _mapPanel;
        
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _resurses;
        [SerializeField] private Image _icon;
        private Upgrade _upgrade;
        public Action OnApplyClicked;
        
        [SerializeField] private Camera _upgradeCamera;
        [SerializeField] private Camera _menuCamera;
        private ICurrencyHandler _currencyHandler;
        private IGameEventBroadcaster _eventBroadcaster;
        protected override  void OnEnabled()
        {
            _mapPanel.SetActive(false);
            _infoPanel.SetActive(false);
            _backButton.onClick.AddListener(()=>ShowApplyWindow(false));
            _applyButton.onClick.AddListener(OnClickApply);
            _currencyHandler = AllServices.Container.Single<ICurrencyHandler>();
            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _eventBroadcaster.OnMoneyChanged += ChangeResurse;
        }

        private void ChangeResurse(int money)
        {
            _resurses.text = money.ToString();
        }

        private void OnClickApply()
        {
            OnApplyClicked?.Invoke();
            ShowApplyWindow(false);
        }

        protected override void OnDisabled()
        {
            _backButton.onClick.RemoveListener(()=>ShowApplyWindow(false));
            _applyButton.onClick.RemoveListener(OnClickApply);
        }
        public void SwitchState(bool isActive)
        {
            _upgradePanel.SetActive(isActive);
            _mapPanel.SetActive(isActive);
            _upgradeCamera.gameObject.SetActive(isActive);
            _menuCamera.gameObject.SetActive(!isActive);
            
            if (isActive)
            {
                _resurses.text = AllServices.Container.Single<ICurrencyHandler>().GetCurrentMoney().ToString();
            }
            else
            {
                _resurses.text = "";
                Reset();
            }
        }

        public void ShowApplyWindow(bool isActive)
        {
            _infoPanel.SetActive(isActive);
            _mapPanel.SetActive(!isActive);
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
            _applyButton.gameObject.SetActive(!_upgrade.IsPurchased);
            
        }
    }
}