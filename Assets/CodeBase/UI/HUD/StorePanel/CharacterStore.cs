using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace UI.HUD.StorePanel
{
    public class CharacterStore : MonoCache
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private PricePanel _pricePanel;
        [SerializeField] private CharacterInfoPanel _characterInfoPanel;
        [SerializeField] private List<GameObject> _charactersObject;
        [SerializeField] private GameObject _characterSlotPrefab;
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _panelCharacterStore;
        [SerializeField] private CharacterGroupContent _characterGroupContent;
        [SerializeField] private CharacterGroup _characterGroup;

        [SerializeField] private List<GameObject> _ordinarySoldier;
        [SerializeField] private List<GameObject> _sergeant;
        [SerializeField] private List<GameObject> _grenader;
        [SerializeField] private List<GameObject> _scout;
        [SerializeField] private List<GameObject> _sniper;
        [SerializeField] private List<GameObject> _turret;
        [SerializeField] private List<GameObject> _humanoid;
        
        private List<List<GameObject>> _characterSkinnedMeshes;

        public Action<CharacterData> BuyCharacter;
        public Action OnMoneyEmpty;
        public Action OnUpdateBought;
        public Action OnUpdateSelectedCharacter;
        public Action <bool> OnReachLimitCharacter;
        private List<Character> _allAvailableHumanoid = new();
        private List<CharacterData> _charactersData = new();
        private List<int> _indexAvailableHumanoid = new();
        private List<CharacterSlot> _characterSlots = new();
        private CharacterSlot _selectedCharacterSlot;
        private CharacterData _selectedCharacter;
        private Store _store;
        private Wallet _wallet;
        private bool _isAvailable;
        public CharacterSlot SelectedCharacterSlot => _selectedCharacterSlot;
        public CharacterData SelectedCharacter => _selectedCharacter;
        public List<List<GameObject>> CharacterSkinnedMeshes => _characterSkinnedMeshes;

        private int _maxCount=3;
        private IGameEventBroadcaster _eventBroadcaster;
        private ICharacterHandler _characterHandler;
        private IUpgradeTree _upgradeTree;
        public void Initialize(Store store)
        {
            _store = store;
            _characterSkinnedMeshes = new List<List<GameObject>>();
            //FillCharacters();
            _characterGroup.Initialize(this);
            _pricePanel.Initialize(this);
            SetCharacters();
            InitializeCharacterSlots();
            InitializeButton();
            _store.IsStoreActive += OpenCharacterStore;
            _wallet = store.GetWallet();
            _characterInfoPanel.Initialize(this);

            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _characterHandler=AllServices.Container.Single<ICharacterHandler>();
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
            AddListener();
        }

        private void InitializeButton()
        {
            _buyButton.onClick.AddListener(OnTryBuyCharacter);
        }

        private void InitializeCharacterSlots()
        {
            _characterGroupContent.Initialize(_characterSlotPrefab);

            foreach (CharacterData data in _charactersData)
            {
                if (data!=null)
                {
                    CharacterSlot characterSlot = Instantiate(_characterSlotPrefab, _characterGroupContent.transform)
                        .GetComponent<CharacterSlot>();
                    characterSlot.Initialize(data, this);
                    _characterSlots.Add(characterSlot);
                    characterSlot.Selected += SetSelectedSlot;
                    if (data.Type==CharacterType.Turret&&!_isAvailable)
                    {
                        characterSlot.SetLock(true);
                    }
                }
            }

            SetStartParametrs();
        }

        private void SetStartParametrs()
        {
            _selectedCharacterSlot = _characterSlots[0];
            _selectedCharacter = _charactersData[0];
        }


        private void OnTryBuyCharacter()
        {
            if (OnTryBuy(_selectedCharacter.Price))
            {
                BuyCharacter?.Invoke(_selectedCharacter);
            }
            else
            {
                OnMoneyEmpty?.Invoke();
            }
        }

        private bool OnTryBuy(int price)
        {
            if (_wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetCharacters()
        {
            CharacterType[] types = (CharacterType[])Enum.GetValues(typeof(CharacterType));
            
            for (int i = 0; i < types.Length; i++)
            {
                string path = AssetPaths.CharactersData + types[i];
                CharacterData data = Resources.Load<CharacterData>(path);
                _charactersData.Add(data);
               
            }

        }

        private void SetSelectedSlot(CharacterSlot characterSlot)
        {
            _selectedCharacterSlot = characterSlot;
                _selectedCharacter =
                    _charactersData.FirstOrDefault(character => character.Type == _selectedCharacterSlot.Type);
                OnUpdateSelectedCharacter?.Invoke();
        }

        private void OpenCharacterStore(bool isActive)
        {
            if (isActive)
            {
                _container.gameObject.SetActive(true);
                SetStartParametrs();
                SetSelectedSlot( _characterSlots[0]);
            }
            else
            {
                _container.gameObject.SetActive(false);
            }
        }
        
        private void AddListener()
        {
            _eventBroadcaster.OnSetActiveHumanoid += CheckState;
            _eventBroadcaster.OnHumanoidDie+=CheckState;
        }

        private void CheckState()
        {
            if (_characterHandler.GetActiveCharacters().Count == _maxCount)
            {
                OnReachLimitCharacter?.Invoke(true);
            }
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
        private void RemoveListener()
        {
            _eventBroadcaster.OnSetActiveHumanoid -= CheckState;
            _eventBroadcaster.OnHumanoidDie-=CheckState;
        }
        
        private void SetUpgrades()
        {
            var i=0;
            
            UpdateUpgradeValue(UpgradeGroupType.Squad,UpgradeType.IncreaseSquadSize, value => _maxCount = value);
            UpdateUpgradeValue(UpgradeGroupType.Turrets,UpgradeType.Turret, value => i = value);
             _isAvailable = i>0;
        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
    }
}