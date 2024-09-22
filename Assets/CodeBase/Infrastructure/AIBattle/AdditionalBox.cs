using System.Collections.Generic;
using Data;
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
        public List<ItemType> GetItemsType() => _types;
        public BoxType GetType() => _boxType;

        public void Initialize(BoxData boxData)
        {
            _boxType = boxData.Type;
            _types = boxData.ItemTypes;
        }
        
        public void SetItems(List<BaseItem> items)
        {
            _items = items;
        }
    }
}
