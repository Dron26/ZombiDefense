using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UI.Unit;
using UnityEngine;

namespace UI.BuyAndMerge.Raid
{
    [DisallowMultipleComponent]
    public class RaidUnitGroup : MonoCache
    {
        private List<GameObject> _slots = new();
        private DragAndDropController _controller;

        public bool IsPlatoonReady => CheckPlatoonState();

        public void Initialize(List<GameObject> slots, DragAndDropController controller)
        {
            _controller = controller;
            FillStartSlot(slots);
        }

        public List<HumanoidUI> GetPlatoon()
        {
            List<HumanoidUI> platoon = new List<HumanoidUI>();

            foreach (var slot in _slots)
            {
                if (slot.GetComponentInChildren<HumanoidUI>() == null) continue;
                HumanoidUI called = slot.GetComponentInChildren<HumanoidUI>();
                platoon.Add(called);
            }

            return platoon;
        }

        private bool CheckPlatoonState()
        {
            bool isPlatooReady = false;

            foreach (GameObject slot in _slots)
            {
                if (slot.GetComponentInChildren<HumanoidUI>() != null)
                {
                    isPlatooReady = true;
                }
            }

            return isPlatooReady;
        }

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
    }
}