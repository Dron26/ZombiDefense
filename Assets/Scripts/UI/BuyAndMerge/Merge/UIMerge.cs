using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using Service.SaveLoadService;
using UI.BuyAndMerge.Raid;
using UI.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.BuyAndMerge.Merge
{
    [DisallowMultipleComponent]
    public class UIMerge : MonoCache
    {
        public bool IsMerged => _isMerged;
        
        private Vector3 _tempPoint;
        private List<HumanoidUI> _humanoids = new();
        private DragAndDropController _controller;
        private SaveLoad _saveLoad;
        private MergePanelInitializer _mergePanel;
        private MergeUnitGroup _mergeUnitGroup;
        private bool _isMerged;

        public void Initialize(List<UIUnit> units, DragAndDropController controller, SaveLoad saveLoad,
            MergePanelInitializer mergePanel)
        {
            _mergePanel = mergePanel;
            _mergeUnitGroup = _mergePanel.GetMergeUnitGroup();
            _controller = controller;
            _saveLoad = saveLoad;
            
            foreach (UIUnit unit in units)
            {
                if (unit.TryGetComponent(out HumanoidUI humanoid))
                {
                    _humanoids.Add(humanoid);
                }
            }
        }

        private GameObject Merge(HumanoidUI draggingHumanoid, HumanoidUI intoHumanoid)
        {
            _tempPoint = intoHumanoid.gameObject.GetComponent<RectTransform>().position;
            int levelMerge = intoHumanoid.GetLevel();
            levelMerge++;

            Transform intoParent = intoHumanoid.transform.parent;

            if (!draggingHumanoid.TryGetComponent(out UnitForBuy _))
            {
                Destroy(draggingHumanoid.gameObject);
            }

            Destroy(intoHumanoid.gameObject);


            foreach (HumanoidUI humanoid in _humanoids)
            {
                if (humanoid.GetLevel() == levelMerge)
                {
                    HumanoidUI human = Instantiate(humanoid, intoParent);
                    human.gameObject.GetComponent<RectTransform>().position = _tempPoint;
                    SetComponents(human.gameObject, humanoid.gameObject);
                    human.gameObject.SetActive(true);
                    return human.gameObject;
                }
            }

            return null;
        }

        public void SetUnitToSlot(PointerEventData dragging, GameObject dropZone)
        {
            int factionNumber = 2;
            _isMerged = false;
            GameObject newUnit;

            if (dropZone.TryGetComponent(out UnitSlot slot) && slot.IsBusy == false)
            {
                if (dragging.pointerDrag.TryGetComponent(out UnitForBuy unitForBuy))
                {
                    if (dragging.pointerDrag.GetComponent<UIUnit>().FactionNumber != factionNumber)
                    {
                        int price = dragging.pointerDrag.GetComponent<HumanoidUI>().GetPrice();

                       // _saveLoad.SpendMoney(price);
                    }

                    newUnit = Instantiate(unitForBuy.gameObject, dropZone.transform);
                    SetComponents(newUnit, dragging.pointerDrag.gameObject);
                }
                else
                {
                    newUnit = dragging.pointerDrag.gameObject;
                    newUnit.transform.SetParent(dropZone.transform);
                    newUnit.GetComponent<UIUnit>().OnIdle(true);
                    newUnit.GetComponent<UIUnit>().SetPriceChildActive(false);
                    dragging.pointerDrag = null;
                }

                if (dropZone.transform.parent.TryGetComponent(out RaidUnitGroup _))
                {
                    if (!newUnit.TryGetComponent(out UnitForBattel _)) newUnit.AddComponent<UnitForBattel>();
                    Destroy(newUnit.GetComponent<UnitForMerge>());
                    Destroy(newUnit.GetComponent<DropObject>());
                }
                else if (newUnit.TryGetComponent(out UnitForBattel _))
                {
                    Destroy(newUnit.GetComponent<UnitForBattel>());
                    newUnit.AddComponent<UnitForMerge>();
                    newUnit.AddComponent<DropObject>().SetController(_controller);
                }

                _isMerged = true;
                dropZone.GetComponent<UnitSlot>().SetBusy(true);
                newUnit.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                if (!dropZone.transform.parent.TryGetComponent(out RaidUnitGroup _))
                {
                    HumanoidUI draggingHumanoid = dragging.pointerDrag.GetComponent<HumanoidUI>();
                    HumanoidUI intoHumanoid = dropZone.GetComponent<HumanoidUI>();

                    if (draggingHumanoid.GetLevel() != intoHumanoid.GetLevel()) return;
                    newUnit = Merge(draggingHumanoid, intoHumanoid);
                    newUnit.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    newUnit.transform.SetSiblingIndex(0);
                    dropZone.GetComponentInParent<UnitSlot>().SetBusy(true);
                    _isMerged = true;
                }
            }
        }
        
       

        public void SetUnit(GameObject donor)
        {
            GameObject slot = _mergeUnitGroup.GetEmptySlot();
            GameObject newUnit = Instantiate(donor, slot.transform);
            slot.GetComponent<UnitSlot>().SetBusy(true);
            SetComponents(newUnit, donor);
        }

        public void SetComponents(GameObject newUnit, GameObject donor)
        {
            int priorityHeroes = 2;
            newUnit.AddComponent<DropObject>();
            newUnit.GetComponent<DropObject>().SetController(_controller);
            newUnit.AddComponent<DragObject>();
            newUnit.GetComponent<DragObject>().SetController(_controller);

            HumanoidUI humanoid = newUnit.GetComponent<HumanoidUI>();
            UIUnit unit = donor.GetComponent<UIUnit>();

            if (newUnit.TryGetComponent(out UnitForBuy _)) 
                Destroy(newUnit.GetComponent<UnitForBuy>());

            int price = unit.FactionNumber != priorityHeroes ? humanoid.GetPrice() : 0;
            newUnit.GetComponent<UIUnit>().Initialize(unit.FactionNumber, unit.Name, price, unit.PriorityNumber);
            newUnit.GetComponent<UIUnit>().OnIdle(true);
            newUnit.GetComponent<UIUnit>().SetPriceChildActive(false);
            newUnit.transform.SetSiblingIndex(0);
        }
        
        
    }
}