using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UI.HUD.Store;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterStore :MonoCache
    {
        //[SerializeField] private GameObject _visualPanel;
        //[SerializeField] private GameObject _infoCharacterPanel;
        //[SerializeField] private GameObject _upgradePanel;
        [SerializeField] private PricePanel _pricePanel;
       // [SerializeField] private CharacterGroupPanel _characterGroupPanel;

        [SerializeField] private List<GameObject> _characters;
        [SerializeField] private GameObject _characterSlotPrefab;

         private List<Humanoid> _allHumanoid=new();
         private List<Humanoid> _allAvailableHumanoid=new();
        
        [SerializeField] private CharacterGroupContent _characterGroupContent;
        [SerializeField] private CharacterSkinnedMeshesGroup _characterSkinnedMeshesGroup;
        [SerializeField] CharacterStoreRotation _characterStoreRotation;
        private List<int> _indexAvailableHumanoid=new();
        private List<CharacterSlot> _characterSlots=new();
        private CharacterSlot _selectedCharacterSlot;
        private SaveLoad _saveLoad;
        private Store _store;
        [SerializeField]private CharacterInfoPanel _characterInfoPanel;
        
        public void Initialize( SaveLoad saveLoad,Store store )
        {
            _saveLoad=saveLoad;
            _store = store;
            _store.IsStoreActive +=SetCharacterData ;
            SetHumanoid();
            InitializeCharacterSlots();
           
            InitializeUpgradePanel();
            SetCharacterData(false);
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
            
        }
        
       
        private void InitializeCharacterConteiner()
        {
            
        }
        

        public List<Humanoid> GetCharacters()
        {
            return _allHumanoid;
        }

        private void SetHumanoid()
        {
            
            foreach (GameObject character in _characters)
            {
                if (character.TryGetComponent(out Humanoid humanoid))
                {
                    humanoid.LoadPrefab();
                    
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
            }
            
            SetCharacterData(true);
        }

        private void SetCharacterData(bool isActive)
        {
            if (isActive)
            {
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
            }
        }
    }
}