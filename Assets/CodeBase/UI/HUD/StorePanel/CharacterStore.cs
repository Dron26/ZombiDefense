using System;
using System.Collections.Generic;
using Data.Upgrades;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace UI.HUD.StorePanel
{
    public class CharacterStore :MonoCache
    {
        //[SerializeField] private GameObject _visualPanel;
        //[SerializeField] private GameObject _infoCharacterPanel;
        //[SerializeField] private GameObject _upgradePanel;
       // [SerializeField] private CharacterGroupPanel _characterGroupPanel;
       
        [SerializeField] UpgradGrouper _upgradGrouper;
        [SerializeField] private Button _buyButton;
        [SerializeField] private PricePanel _pricePanel;
        [SerializeField]private CharacterInfoPanel _characterInfoPanel;

        [SerializeField] private List<GameObject> _characters;
        [SerializeField] private GameObject _characterSlotPrefab;
        [SerializeField] private GameObject _container;

         private List<Humanoid> _allHumanoid=new();
         private List<Humanoid> _allAvailableHumanoid=new();
        
        [SerializeField] private CharacterGroupContent _characterGroupContent;
        [SerializeField] private CharacterSkinnedMeshesGroup _characterSkinnedMeshesGroup;
        [SerializeField] CharacterStoreRotation _characterStoreRotation;
       
        public Action<Humanoid> OnCharacterBought;

        private List<int> _indexAvailableHumanoid=new();
        private List<CharacterSlot> _characterSlots=new();
        private CharacterSlot _selectedCharacterSlot;
        private Humanoid _selectedHumanoid;
        private SaveLoadService _saveLoadService;
        private Store _store;
        private MoneyData _moneyData;
        public Action OnUpdateBought;
        
        public void Initialize( SaveLoadService saveLoadService,Store store ,MoneyData moneyData )
        {
            _moneyData=moneyData;
            _saveLoadService=saveLoadService;
            _store = store;
            _store.IsStoreActive +=SetCharacterData ;
            SetHumanoid();
            InitializeCharacterSlots();
           
                //  InitializeUpgradePanel();
            SetCharacterData(false);

            InitializeButton();
        }

        private void InitializeButton()
        {
            _buyButton.onClick.AddListener(OnTryBuyCharacter);
        }
        
        public void OpenPanel()
        {
            //_saveLoad.UpdateData();
        }
        
        private void InitializeCharacterSlots()
        {
            _characterGroupContent.Initialize(_characterSlotPrefab);
            
            foreach (Humanoid humanoid in _allHumanoid)
            {
                
                CharacterSlot characterSlot = Instantiate(_characterSlotPrefab,_characterGroupContent.transform).GetComponent<CharacterSlot>();
                characterSlot.Initialize(humanoid,this);
                _characterSlots.Add(characterSlot);
                characterSlot.Selected+=SetSelectedSlot;
            }
            
            _selectedCharacterSlot= _characterSlots[0];
            _selectedHumanoid=_selectedCharacterSlot.Humanoid;
            
        }

        private void SetPriceInfo()
        {
            _pricePanel.SetInfo(_selectedCharacterSlot.Price);
        }
        
        private void SetVisual()
        {
            _characterSkinnedMeshesGroup.ShowCharacter(_selectedCharacterSlot.Index);
        }
        
        private void SetCharacterInfo()
        {
            _characterInfoPanel.SetParametrs(_selectedCharacterSlot.Humanoid);
        }
        
        private void InitializeUpgradePanel()
        {
            _upgradGrouper.Initialize(_saveLoadService,this);
            _upgradGrouper.OnBuyUpgrade+=OnTryBuyUpgrade;
        }

        private void OnTryBuyUpgrade(UpgradeData upgradeData,int price,int level)
        {
            if (OnTryBuy(price))
            {
                UpdateParametrs(upgradeData,level);
            }
        }
        
        private void OnTryBuyCharacter()
        {
            if (OnTryBuy(_selectedHumanoid.GetPrice()))
            {
                OnCharacterBought?.Invoke(_selectedHumanoid);
                
            }
        }
        
        private bool OnTryBuy(int price)
        {
            if (_moneyData.IsMoneyEnough(price))
            {
                _moneyData.SpendMoney(price);
                return true;
            }
            else
            {
                print("должен мигать кошелек");
                return false;
            }
        }

        private void UpdateParametrs(UpgradeData upgradeData,int level)
        {
            _selectedHumanoid.SetUpgrade(upgradeData, level);
            OnUpdateBought?.Invoke();
        }

        private void SetHumanoid()
        {
            foreach (GameObject character in _characters)
            {
                if (character.TryGetComponent(out Humanoid humanoid))
                {
                    _allHumanoid.Add(humanoid);
                    
                    if (humanoid.IsBuyed)
                    {
                        _allAvailableHumanoid.Add(humanoid);
                        _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(humanoid));
                    }
                }    
                
            }
        }
        
        public void SetSelectedSlot(CharacterSlot characterSlot)
        {
            
            if (characterSlot!=_selectedCharacterSlot)
            {
                _selectedCharacterSlot = characterSlot;
                _selectedHumanoid=_selectedCharacterSlot.Humanoid;
                SetCharacterData(true);
            }
        }

        private void SetCharacterData(bool isActive)
        {
            if (isActive)
            {
                _container.gameObject.SetActive(true);
                _characterStoreRotation.gameObject.SetActive(true);
                _characterStoreRotation.Rotate();
                SetPriceInfo();
                SetVisual();
                SetCharacterInfo();
            }
            else
            {
                _characterStoreRotation.StopRotation();
                _characterStoreRotation.gameObject.SetActive(false);
                _container.gameObject.SetActive(false);
                
            }
        }
    }
}