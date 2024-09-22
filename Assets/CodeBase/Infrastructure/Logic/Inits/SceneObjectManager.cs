using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoriesBox;
using Infrastructure.Factories.FactoryWarriors.Humanoids;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SceneObjectManager : MonoCache
{
    private BoxStore _boxStore;
    private CharacterStore _characterStore;
    private Store _store;
    private BoxFactory _boxFactory;
    private HumanoidFactory _humanoidFactory;
    private CharacterFactory _characterFactory;
    private ItemFactory _itemFactory;
    private PlayerCharacterInitializer _characterInitializer; 
    private AudioManager _audioManager;
    public UnityAction<Character> CreatedHumanoid;
    public UnityAction<AdditionalBox> BuildedBox;
    private WorkPoint _selectedWorkPoint;

    public void Initialize( CharacterStore characterStore,BoxStore boxStore)
    {
        _boxStore = boxStore;
        _characterStore = _characterStore;
        // Подписка на события магазина
        boxStore.BuyBox += HandleBox;
        characterStore.OnCharacterBought += HandleCharacterSelected;
    }

    private void HandleBox(BoxData boxData,Transform pointTransform)
    {
        AdditionalBox box = BuildBox(boxData);
        PlaceBoxOnScene(box.transform,pointTransform);
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

    private void HandleCharacterSelected(CharacterData characterData)
    {
        BuildCharacter(characterData);
    }

    private void BuildCharacter(CharacterData characterData)
    {
        GameObject prefab = _characterFactory.Create(characterData.Type);
        Character character = prefab.GetComponent<Character>();
        character.OnInitialize += OnBuildedCharacter;
        character.SetAudioManager(_audioManager);
        
        WeaponController weaponController  = prefab.GetComponent<WeaponController>();
        weaponController.Initialize();
        
        Transform characterTransform = prefab.transform;
        SetTransformParametrs(characterTransform);
        character.Initialize(characterData);
    }

    private void SetTransformParametrs(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.tag="PlayerUnit";
        float randomAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
    }
    
    private void OnBuildedCharacter( Character character)
    {
        CreatedHumanoid?.Invoke(character);
    }
    
    private void OnBuildedBox( AdditionalBox box)
    {
        BuildedBox?.Invoke(box);
    }
        
    public void Initialize( AudioManager audioManager)
    {
        _audioManager=audioManager;
    }

    private void PlaceBoxOnScene(Transform boxTransform,Transform pointTransform)
    {
        boxTransform.parent = pointTransform;
        boxTransform.position = pointTransform.position;
    }

    private void OnSelectPoint(WorkPoint workPoint)
    {
        _selectedWorkPoint = workPoint;
    }
    private void OnDestroy()
    {
    }
    
    private void AddListener()
    {
        _saveLoadService.OnSelectedNewPoint += OnSelectPoint;
        _additionalEquipmentButton.OnSelectedMedicineBox += OnSelectMedicineBox;
        _additionalEquipmentButton.OnSelectedWeaponBox += OnSelectSmallWeaponBox;
    }

    private void RemoveListener()
    {
        _saveLoadService.OnSelectedNewPoint -= OnSelectPoint;

        _additionalEquipmentButton.OnSelectedMedicineBox -= OnSelectMedicineBox;
        _additionalEquipmentButton.OnSelectedWeaponBox -= OnSelectSmallWeaponBox;
    }
}



namespace Infrastructure.Factories.FactoryWarriors.Humanoids
{
}