using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using Service.SaveLoadService;
using UI.Unit;
using UnityEngine;

namespace UI.BuyAndMerge.Merge
{
    [DisallowMultipleComponent]
    public class MergeUnitGroup : MonoCache
    {
        private List<GameObject> _slots = new();
        private DragAndDropController _controller;
        private GameObject _emptySlot;
        private SaveLoad _saveLoad;

        public void Initialize(List<GameObject> slots, DragAndDropController controller, SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _controller = controller;
            
            FillStartSlot(slots);
        }

        public bool HaveEmptySlot()
        {
            foreach (GameObject slot in _slots)
            {
                if (!slot.GetComponent<UnitSlot>().IsBusy)
                {
                    _emptySlot = slot;
                    return true;
                }
            }

            return false;
        }

        public GameObject GetEmptySlot() => _emptySlot;

        private void FillStartSlot(List<GameObject> slots)
        {
            int number = 0;

            foreach (GameObject slot in slots)
            {
                GameObject newSlot = Instantiate(slot, transform);
                newSlot.GetComponent<DropObject>().SetController(_controller);
                newSlot.GetComponent<UnitSlot>().SetNumber(number);
                _slots.Add(newSlot);
            }
        }

        protected override void OnDisabled()
        {
            if (!_saveLoad.GetStartBattle())
            {
                _saveLoad.ApplyMoney(GetPrice());
            }
        }

        public int GetPrice()
        {
            int price = 0;
            
            foreach (GameObject slot in _slots)
            {
                HumanoidUI humanoid =slot.GetComponentInChildren<HumanoidUI>();
                if (humanoid!=null)
                {
                    price+= humanoid.GetPrice();
                }
            }

            return price;
        }
    }
}