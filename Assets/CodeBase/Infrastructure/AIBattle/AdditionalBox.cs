using System.Collections.Generic;
using Data;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;

namespace Infrastructure.AIBattle
{
    public class AdditionalBox : MonoCache
    {
        private List<BaseItem> _items;
        private List<ItemType> _types;
        private BoxType _boxType;
        private BoxData _boxData;
        public List<BaseItem> GetItems() => _items;
        public List<ItemType> GetItemsType() => _types;
        public BoxData GetData() => _boxData;

        public void Initialize(BoxData boxData)
        {
            _boxData=boxData;
            _boxType = _boxData.Type;
            _types = _boxData.ItemTypes;
        }
        
        public void SetItems(List<BaseItem> items)
        {
            _items = items;
            foreach (var item in _items)
            {
                item.gameObject.SetActive(false);
                item.transform.parent=transform;
            }
           
        }
    }
}