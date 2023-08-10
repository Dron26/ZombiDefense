using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.ADS;
using Service.SaveLoadService;
using UI.BuyAndMerge.Merge;
using UI.Panel;
using UI.Unit;
using UI.WarningWindow;
using UnityEngine;
using UnityEngine.Events;

namespace UI.BuyAndMerge
{
    public class CharacterSeller : MonoCache
    {
        public GameObject GetAdViwer() => _adViwer;
        public UnityAction ChangeMoney;
     
        private SaveLoadService _saveLoadService;
        private UIMerge _uiMerge;
        private GameObject _adViwer;
        private PanelUnitForReward _rewardPanel;
        private MergeUnitGroup _mergeUnitGroup;
        private WindowSwither _windowSwither;
        
        private readonly int _factionNumber = 2;
        private int _unitFactionNumber;

        public void Initialize(SaveLoadService saveLoadService, ADCanvas adCanvas, UIMerge uiMerge,MergeUnitGroup mergeUnitGroup,WindowSwither windowSwither)
        {
            _windowSwither = windowSwither;
            _mergeUnitGroup = mergeUnitGroup;
            _saveLoadService = saveLoadService;
            _rewardPanel = adCanvas.GetComponentInChildren<PanelUnitForReward>();
            _uiMerge =uiMerge ;
        }

        public void BuyCharacter(GameObject character)
        {
            _unitFactionNumber = character.GetComponent<UIUnit>().FactionNumber;

            if (character.TryGetComponent(out UnitForBuy unitForBuy))
            {
                if (_unitFactionNumber != _factionNumber)
                {
                    int price = unitForBuy.GetComponent<HumanoidUI>().GetPrice();
                    int money = _saveLoadService.ReadAmountMoney();

                    if ( CanBuy(price,money))
                    {
                        Buy(price);
                        SetUnitToSlot(character);
                    }
                }
                else
                {
                    _rewardPanel.SelectPanel();
                    _adViwer = character;
                }
            }
        }

        private void SetUnitToSlot(GameObject buyed)
        {
            GameObject slot = _mergeUnitGroup.GetEmptySlot();
            GameObject newUnit = Instantiate(buyed, slot.transform);
            slot.GetComponent<UnitSlot>().SetBusy(true);
            _uiMerge.SetComponents(newUnit, buyed);
        }

        private bool CanBuy(int price, int money)
        {
            if (!_mergeUnitGroup.HaveEmptySlot())
            {
                _windowSwither.ShowWindow(0);
                return false;
            }
    
            if (price > money)
            {
                _windowSwither.ShowWindow(2);
                return false;
            }
    
            return true;
        }


        private void Buy(int price)
        {
     //       _saveLoad.SpendMoney(price);
            ChangeMoney?.Invoke();
        }
    }
}