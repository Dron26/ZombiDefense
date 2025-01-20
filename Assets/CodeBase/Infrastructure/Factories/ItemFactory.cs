using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class ItemFactory : MonoCache
    {
        public T Create<T>(ItemType type) where T : BaseItem
        {
            string itemDataPath = AssetPaths.ItemsData + type;
            ItemData itemData = Resources.Load<ItemData>(itemDataPath);
            string prefabPath = GetPrefabPath(itemData.Type.ToString(), itemData.IsMedicine ? BoxType.Equipment : BoxType.SmallWeapon);
            GameObject newItemObject = Instantiate(Resources.Load<GameObject>(prefabPath));
            T newItem = newItemObject.GetComponent<T>();

            if (newItem != null)
            {
                newItem.Initialize(itemData);
            }
            else
            {
                Debug.LogError($"Failed to get component {typeof(T).Name} from instantiated object.");
                Destroy(newItemObject);
            }

            return newItem;
        }

        private string GetPrefabPath(string itemName, BoxType boxType)
        {
            switch (boxType)
            {
                case BoxType.SmallWeapon:
                    return $"{AssetPaths.WeaponPrefabs}{itemName}";
                case BoxType.Equipment:
                    return $"{AssetPaths.EquipmentPrefabs}{itemName}";
                default:
                    Debug.LogWarning($"Unknown ItemGroup: {boxType}");
                    return string.Empty;
            }
        }
    }
}