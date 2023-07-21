using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.ADS;
using Service.SaveLoadService;
using UI.BuyAndMerge.Merge;
using UI.Empty.Slot;
using UI.Panel;
using UI.Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Service.DragAndDrop
{
    [DisallowMultipleComponent]
    public class DragAndDropController : MonoCache
    {
        [SerializeField] private UIMerge _merging;
        [SerializeField] private TempSlotUI _tempSlotUI;
        private SaveLoad _saveLoad;

        private Transform DefaultParent;
        private Transform DefaultUnitParent;
        private UnitGroup _unitGroup;
        private UnitSlot _unitSlot;
        private bool _isTakeUnitForBuy;
        private Camera _mainCamera;
        private Vector3 _offset;
        private int _defaulIndex;
        private bool _isMerge;
        private bool _isCollect;
        private bool _isCanBuy;
        private bool _isBuyHeroes;
        private bool _isClickHeroes;
        private Vector3 _newPos;
        private PanelUnitForReward _rewardPanel;
        private readonly int factionNumber = 2;
        private int _unitFactionNumber;
        public void Initialize(SaveLoad saveLoad, ADCanvas adCanvas)
        {
            _saveLoad = saveLoad;
            _mainCamera = Camera.allCameras[0];
            _rewardPanel = adCanvas.GetComponentInChildren<PanelUnitForReward>();
        }

        public void OnBeginDrag(PointerEventData eventData, GameObject dragged)
        {
            _isCanBuy = true;
            _isMerge = false;
            _isCollect = false;
            int factionNumber = 2;

             dragged.GetComponentInChildren<UnitHighlight>().SetHighlighted(true);
            
            _offset = dragged.transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);
            Transform parent = dragged.transform.parent;
            DefaultParent = parent;
            DefaultUnitParent = parent;
            _defaulIndex = dragged.transform.GetSiblingIndex();
            _unitFactionNumber = dragged.GetComponent<UIUnit>().FactionNumber;

            if (dragged.TryGetComponent(out UnitForBuy unitForBuy))
            {
                if (_unitFactionNumber != factionNumber)
                {
                    int price = unitForBuy.GetComponent<HumanoidUI>().GetPrice();
                    int money = _saveLoad.ReadAmountMoney();

                    if (price > money)
                    {
                        _isCanBuy = false;
                        eventData.pointerDrag = null;
                    }
                    else
                    {
                        _isTakeUnitForBuy = true;
                        _unitGroup = dragged.GetComponentInParent<UnitGroup>();
                        _unitGroup.SetEmptySlot(dragged.transform.GetSiblingIndex());
                    }
                }
                else
                {
                    _isBuyHeroes = true;
                    _rewardPanel.SelectPanel();
                }
            }
            else
            {
                _unitSlot = dragged.GetComponentInParent<UnitSlot>();

                if (dragged.TryGetComponent(out DropObject dropObject)) dropObject.enabled = false;

                if (_unitFactionNumber == factionNumber) _isClickHeroes = true;

                _unitSlot.SetBusy(false);
            }
            
            if (_isCanBuy)
            {
                dragged.transform.SetParent(_tempSlotUI.transform);
                dragged.transform.SetAsLastSibling();
                dragged.GetComponent<CanvasGroup>().blocksRaycasts = false;
                dragged.GetComponent<UIUnit>().OnIdle(false);
            }
        }

        public void OnDrag(PointerEventData eventData, GameObject dragged)
        {
            _newPos = _mainCamera.ScreenToWorldPoint(eventData.position);
            _newPos.z = -1f;
            dragged.transform.position = _newPos + _offset;
            
            if (dragged.transform.position.x < -3.8f)
            {
                if (_isTakeUnitForBuy == false)
                {
                    eventData.pointerDrag = null;
                    _newPos.z = 0f;
                    dragged.transform.position = _newPos + _offset;
                    OnEndDrag(dragged);
                }
            }
        }

        public void OnEndDrag(GameObject dragged)
        {
            
            dragged.GetComponentInChildren<UnitHighlight>().SetHighlighted(false);
            dragged.transform.SetParent(DefaultParent);
            dragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
            dragged.transform.SetSiblingIndex(_defaulIndex);

            if (_isTakeUnitForBuy)
            {
                _unitGroup.SetEmptySlot(_defaulIndex);
                _isTakeUnitForBuy = false;
                dragged.GetComponent<UIUnit>().OnIdle(true);
            }
            else
            {
                if (dragged.TryGetComponent(out DropObject dropObject)) dropObject.enabled = true;

                if (_isMerge == true && _isCollect == true)
                    _unitSlot.SetBusy(false);
                else if (_isBuyHeroes == false || _isClickHeroes) _unitSlot.SetBusy(true);
            }

            _newPos.z = 0f;
            dragged.transform.position = _newPos + _offset;
        }

        public void OnDrop(PointerEventData eventData, GameObject picker)
        {
            eventData.pointerDrag.GetComponentInChildren<UnitHighlight>().SetHighlighted(false);
            
            
            if (_unitFactionNumber == factionNumber)
            {
                if (picker.TryGetComponent(out UnitSlot _)) CollectUnit(eventData, picker);
            }
            else
                CollectUnit(eventData, picker);
        }

        private void CollectUnit(PointerEventData drop, GameObject picker)
        {
            if (picker.TryGetComponent(out UnitSlot slot))
            {
                _newPos.z = 0f;
                drop.pointerDrag.transform.position = _newPos + _offset;
                _merging.SetUnitToSlot(drop, slot.gameObject);
                _isCollect = true;
            }
            else if (drop.pointerDrag.transform.TryGetComponent(out HumanoidUI dropHumanoid) &&
                     picker.TryGetComponent(out HumanoidUI pickerObject) &&
                     drop.pointerDrag.TryGetComponent(out UnitForBuy _) == false)
            {
                if (!drop.pointerDrag.transform.TryGetComponent(out UnitForBattel _))
                {
                    _newPos.z = 0f;
                    drop.pointerDrag.transform.position = _newPos + _offset;

                    if (dropHumanoid.GetLevel() != pickerObject.GetLevel())
                    {
                        Transform tempParent = pickerObject.transform.parent;
                        dropHumanoid.gameObject.transform.SetParent(pickerObject.gameObject.transform.parent);
                        OnEndDrag(pickerObject.gameObject);
                        DefaultParent = tempParent;
                        drop.pointerDrag = null;
                        OnEndDrag(dropHumanoid.gameObject);
                    }
                    else
                    {
                        _merging.SetUnitToSlot(drop, pickerObject.gameObject);
                        _isCollect = true;
                    }
                }
            }

            _isMerge = _merging.IsMerged;
        }
    }
}