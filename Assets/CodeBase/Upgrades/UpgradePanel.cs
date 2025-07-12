using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Upgrades
{
    public class UpgradePanel:MonoCache
    {
        [SerializeField] private Button _backButtonInfoPanel;
        [SerializeField] private Button _applyButton;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _mapPanel;
        
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _resurses;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _purchasePanel;
        [SerializeField] private PurchaseYG _purchaseYG;
        private Upgrade _upgrade;
        public event Action OnApplyClicked;
        public event Action OnApplyClickedYG;
        
        [SerializeField] private Camera _upgradeCamera;
        [SerializeField] private Camera _menuCamera;
        private ICurrencyHandler _currencyHandler;
        private IGameEventBroadcaster _eventBroadcaster;

        public void Initialize()
        {
            _currencyHandler = AllServices.Container.Single<ICurrencyHandler>();
            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
        }
        
        public void SetActive(Upgrade upgrade)
        {
            _mapPanel.SetActive(false);
            _infoPanel.SetActive(false);
            
            _upgrade = upgrade;
            _icon.sprite = _upgrade.Icon;
            _name.text  = _upgrade.Name;
            _description.text = _upgrade.Description;
            _price.text ="$ "+ _upgrade.Cost;
            _applyButton.gameObject.SetActive(!_upgrade.IsPurchased);

            
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
            _backButtonInfoPanel.onClick.RemoveListener(()=>ShowApplyWindow(false));
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
            if (isActive)
            {
                SetId();
            }
            
            _infoPanel.SetActive(isActive);
           // _purchasePanel.SetActive(isActive);
            _mapPanel.SetActive(!isActive);
            _purchasePanel.gameObject.SetActive(_upgrade.IsPurchased == false);
        }

        private void SetId()
        {
            _purchaseYG.SetId(_upgrade.GroupType+"_"+_upgrade.Type+"_"+_upgrade.Id);
        }
        
        private void SuccessPurchased(string id)
        {
            OnApplyClickedYG?.Invoke();
            ShowApplyWindow(false);
        }
        
        private void Reset()
        {
            _upgrade = null;
            _icon.sprite = null;
            _name.text  = "";
            _description.text = "";
            _price.text ="";
        }

        private void AddListener()
        {
            _eventBroadcaster.OnMoneyChanged += ChangeResurse;
            _backButtonInfoPanel.onClick.AddListener(()=>ShowApplyWindow(false));
            _applyButton.onClick.AddListener(OnClickApply);
            
            YG2.onPurchaseSuccess += SuccessPurchased;
        }
        
        private void RemoveListener()
        {
            _eventBroadcaster.OnMoneyChanged -= ChangeResurse;
            _backButtonInfoPanel.onClick.RemoveListener(()=>ShowApplyWindow(false));
            _applyButton.onClick.RemoveListener(OnClickApply);
            
            YG2.onPurchaseSuccess -= SuccessPurchased;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}