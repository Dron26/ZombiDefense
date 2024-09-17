using System.Collections.Generic;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class AdditionalBox : MonoCache
    {
        private List<BaseItem> _items;
        private List<ItemType> _types;
        private BoxType _boxType;

        public List<BaseItem> GetItems() => _items;
        public BoxType GetType() => _boxType;

        public void Initialize(BoxData boxData)
        {
            _boxType = boxData.Type;
            _types = boxData.ItemTypes;
            _items = new List<BaseItem>();

            foreach (var itemType in _types)
            {
                // Выбираем путь в зависимости от группы
                string path = GetItemDataPath(itemType);
                ItemData itemData = Resources.Load<ItemData>(path);

                if (itemData != null)
                {
                    // Загрузка префаба предмета на основе имени
                    string prefabPath = GetPrefabPath(itemData.name);
                    GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);

                    if (itemPrefab != null)
                    {
                        // Создание экземпляра предмета и его настройка
                        GameObject newItem = Instantiate(itemPrefab, transform);
                        BaseItem baseItem = newItem.GetComponent<BaseItem>();

                        // Вызов абстрактного метода Initialize, который будет реализован для конкретного типа предмета
                        baseItem.Initialize(itemData);

                        // Добавление предмета в список
                        _items.Add(baseItem);
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab for item {itemData.name} not found at {prefabPath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"ItemData not found at {path}");
                }
            }
        }

        // Метод для получения пути к данным предмета на основе ItemType и ItemGroup
        private string GetItemDataPath(ItemType itemType)
        {
            switch (_boxType)
            {
                case BoxType.SmallWeapon:
                    return $"{AssetPaths.WeaponData}{itemType}";
                case BoxType.Equipment:
                    return $"{AssetPaths.Equipment}{itemType}";
                default:
                    Debug.LogWarning($"Unknown ItemGroup: {_boxType}");
                    return string.Empty;
            }
        }

        // Метод для получения пути к префабу предмета
        private string GetPrefabPath(string itemName)
        {
            switch (_boxType)
            {
                case BoxType.SmallWeapon:
                    return $"{AssetPaths.WeaponPrefabs}{itemName}";
                case BoxType.Equipment:
                    return $"{AssetPaths.EquipmentPrefabs}{itemName}";
                default:
                    Debug.LogWarning($"Unknown ItemGroup: {_boxType}");
                    return string.Empty;
            }
        }
    }
}
