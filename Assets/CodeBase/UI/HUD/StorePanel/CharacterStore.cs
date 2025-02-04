using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
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
        private List<Character> _allAvailableHumanoid = new();
        private List<CharacterData> _charactersData = new();
        private List<int> _indexAvailableHumanoid = new();
        private List<CharacterSlot> _characterSlots = new();
        private CharacterSlot _selectedCharacterSlot;
        private CharacterData _selectedCharacter;
        private Store _store;
        private Wallet _wallet;
        private bool _isInitialized;
        public CharacterSlot SelectedCharacterSlot => _selectedCharacterSlot;
        public CharacterData SelectedCharacter => _selectedCharacter;
        public List<List<GameObject>> CharacterSkinnedMeshes => _characterSkinnedMeshes;

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
            _isInitialized = true;
            _store.IsStoreActive += OpenCharacterStore;
            _wallet = store.GetWallet();
            _characterInfoPanel.Initialize(this);
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
                }
            }

            SetStartParametrs();
        }

        private void SetStartParametrs()
        {
            _selectedCharacterSlot = _characterSlots[0];
            _selectedCharacter = _charactersData[0];
          // _selectedCharacterSlot.Selected.Invoke(_selectedCharacterSlot);
        }

        // private void OnTryBuyUpgrade(UpgradeData upgradeData, int price, int level)
        // {
        //     if (OnTryBuy(price))
        //     {
        //         UpdateParametrs(upgradeData, level);
        //     }
        // }

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

        // private void UpdateParametrs(UpgradeData upgradeData, int level)
        // {
        //     _selectedCharacter.SetUpgrade(upgradeData, level);
        //
        //     if (_selectedCharacter.TryGetComponent(out Humanoid humanoid))
        //     {
        //         HumanoidWeaponController humanoidWeaponController = humanoid.GetComponent<HumanoidWeaponController>();
        //         humanoidWeaponController.SetUpgrade(upgradeData, level);
        //     }
        //
        //     OnUpdateBought?.Invoke();
        // }

        private void SetCharacters()
        {
            CharacterType[] types = (CharacterType[])Enum.GetValues(typeof(CharacterType));
            
            for (int i = 0; i < types.Length; i++)
            {
                string path = AssetPaths.CharactersData + types[i];
                CharacterData data = Resources.Load<CharacterData>(path);
                _charactersData.Add(data);
                //_charactersObject[i].SetActive(false);

                // if (_charactersObject[i].TryGetComponent(out Character character))
                // {
                //    
                //   
                //    character.Initialize(data);
                //    _allAvailableHumanoid.Add(character);
                //    _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(character));
                // }
            }

            // foreach (GameObject obj in _charactersObject)
            // {
            //     if (obj.TryGetComponent(out Character character))
            //     {
            //         string path = AssetPaths.CharactersData + CharactersData;
            //         CharacterData data = Resources.Load<CharacterData>(path);
            //         _charactersData.Add(data);
            //         character.Initialize(data);
            //         _characters.Add(character);
            //         _allAvailableHumanoid.Add(character);
            //         _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(character));
            //     }

            // if (obj.TryGetComponent(out Character character))
            // {
            //     string path = AssetPaths.Characters + obj.name;
            //     CharacterData data = Resources.Load<CharacterData>(path);
            //     character.Initialize(data);
            //
            //     if (obj.TryGetComponent(out Humanoid humanoid))
            //     {
            //         HumanoidWeaponController humanoidWeaponController =
            //             character.GetComponent<HumanoidWeaponController>();
            //         humanoidWeaponController.UIInitialize();
            //     }
            //     else if (obj.TryGetComponent(out Turret turret))
            //     {
            //         TurretWeaponController turretWeaponController =
            //             character.GetComponent<TurretWeaponController>();
            //         turretWeaponController.UIInitialize();
            //     }
            //
            //     
            // }
            // else if (obj.TryGetComponent(out Turret turret))
            // {
            //     _characters.Add(turret);
            //
            //     if (turret.IsBuyed)
            //     {
            //         _allAvailableHumanoid.Add(humanoidj);
            //         _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(humanoidj));
            //     }
            // }
            //
            //
            // if (obj.TryGetComponent(out Humanoid humanoidj))
            // {
            //     HumanoidWeaponController humanoidWeaponController =
            //         humanoidj.GetComponent<HumanoidWeaponController>();
            //     humanoidWeaponController.UIInitialize();
            //     _characters.Add(humanoidj);
            //
            //     if (humanoidj.IsBuyed)
            //     {
            //         _allAvailableHumanoid.Add(humanoidj);
            //         _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(humanoidj));
            //     }
            // }
            // else if (obj.TryGetComponent(out Turret turret))
            // {
            //     _characters.Add(turret);
            //
            //     if (turret.IsBuyed)
            //     {
            //         _allAvailableHumanoid.Add(humanoidj);
            //         _indexAvailableHumanoid.Add(_allAvailableHumanoid.IndexOf(humanoidj));
            //     }
            // }
            //}
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
    }
}