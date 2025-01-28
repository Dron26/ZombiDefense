using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Characters.Robots;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Infrastructure.Points;
using Interface;
using Services;
using Services.Audio;
using Services.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;
using CharacterData = Characters.Humanoids.AbstractLevel.CharacterData;

namespace Infrastructure.Logic.Inits
{
    [RequireComponent(typeof(BoxFactory))]
    [RequireComponent(typeof(CharacterFactory))]
    [RequireComponent(typeof(ItemFactory))]
    public class SceneObjectManager : MonoCache
    {
        private BoxStore _boxStore;
        private CharacterStore _characterStore;
        private BoxFactory _boxFactory;
        private CharacterFactory _characterFactory;
    
        [SerializeField]private ItemFactory _itemFactory;
        private PlayerCharacterInitializer _characterInitializer; 
        private AudioManager _audioManager;
        public UnityAction<Character> CreatedHumanoid;
        public UnityAction<AdditionalBox> BuildedBox;
        private WorkPoint _selectedWorkPoint;
        private Store _store;
        private MovePointController _movePointController;
        private SaveLoadService _saveLoadService;
        public void Initialize(Store store, MovePointController movePointController, AudioManager audioManager,
            SaveLoadService saveLoadService)
        {
            _store=store;
            _movePointController = movePointController;
            _audioManager = audioManager;
            _characterFactory=GetComponent<CharacterFactory>();
            _boxFactory=GetComponent<BoxFactory>();
            _itemFactory=GetComponent<ItemFactory>();
            _saveLoadService = saveLoadService;
            AddListener();
        }

        private void OnSelectedNewPoint(WorkPoint workPoint)
        {
            _selectedWorkPoint = workPoint;
        }

        private void OnBoughtBox(BoxData boxData)
        {
            AdditionalBox box = BuildBox(boxData);
            OnBuildedBox(box);
        }

        private AdditionalBox BuildBox(BoxData boxData)
        {
            GameObject prefabBox = _boxFactory.Create(boxData.Type);
            AdditionalBox box = prefabBox.GetComponent<AdditionalBox>();
            box.Initialize(boxData);
            List<BaseItem> boxItems = new List<BaseItem>();
        
            foreach (ItemType type in boxData.ItemTypes)
            {
                BaseItem item = _itemFactory.Create<BaseItem>(type);
                boxItems.Add(item);
            }
        
            box.SetItems(boxItems);

            return box;
        }

        private void OnBoughtCharacter(CharacterData characterData)
        {
            BuildCharacter(characterData);
        }

        private void BuildCharacter(CharacterData characterData)
        {
            GameObject prefab = _characterFactory.Create(characterData.Type);
            Character character = prefab.GetComponent<Character>();
            character.OnInitialize += OnBuildedCharacter;
            character.SetAudioManager(_audioManager);
        
            Transform characterTransform = prefab.transform;
            SetTransformParametrs(characterTransform);
        
            if (characterData.Type!= CharacterType.Turret)
            {
                WeaponController weaponController  = prefab.GetComponent<WeaponController>();
                weaponController.Initialize(characterData);
                character.Initialize(characterData);
            }
            else
            {
                TurretWeaponController weaponController  = prefab.GetComponent<TurretWeaponController>();
                weaponController.Initialize(characterData);
                Turret turret=prefab.GetComponent<Turret>();
                turret.SetSaveLoadService(_saveLoadService);
                turret.Initialize(characterData);
            }
        }

        private void SetTransformParametrs(Transform transform)
        {
            // transform.localPosition = Vector3.zero;
            transform.tag="PlayerUnit";
            float randomAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
        }
    
        private void OnBuildedCharacter(Character character)
        {
            AllServices.Container.Single<ISearchService>().AddEntity(character);

            CreatedHumanoid?.Invoke(character);
        }
    
        private void OnBuildedBox( AdditionalBox box)
        {
            _selectedWorkPoint.SetWeaponBox(box);
        }
    
        private void PlaceBoxOnScene(Transform boxTransform,Transform pointTransform)
        {
            boxTransform.parent = pointTransform;
            boxTransform.position = pointTransform.position;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    
        private void AddListener()
        {
            _store.OnBoughtCharacter+=OnBoughtCharacter;
            _store.OnBoughtBox+=OnBoughtBox;
            _store.OnBoughtUpgrade+=OnBoughtUpgrade;
            _movePointController.OnClickPoint += OnSelectedNewPoint;
        }

        private void OnBoughtUpgrade(WorkPoint point)
        {
            throw new System.NotImplementedException();
        }

        private void RemoveListener()
        {
            _store.OnBoughtCharacter-=OnBoughtCharacter;
            _store.OnBoughtBox-=OnBoughtBox;
            _store.OnBoughtUpgrade-=OnBoughtUpgrade;
            _movePointController.OnClickPoint += OnSelectedNewPoint;
        }
    }
}