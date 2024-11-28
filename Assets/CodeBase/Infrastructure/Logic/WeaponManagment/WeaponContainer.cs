using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public  class WeaponContainer:MonoCache
    {
        public List<GameObject> _items=new List<GameObject>();
        private ItemType _selectType;
        public void SetItem(ItemType type)
        {
            _selectType = type;
            _items[(int)_selectType].gameObject.SetActive(true);
        }

        public Weapon GetItem()
        {
            return _items[(int)_selectType].gameObject.GetComponent<Weapon>();
        }
    }
}