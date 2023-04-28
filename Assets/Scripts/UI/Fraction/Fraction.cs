using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UI.BuyAndMerge;
using UI.Empty;
using UI.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Fraction
{
    [DisallowMultipleComponent]
    public class Fraction : MonoCache
    {
        public int FactionNumber => _factionNumber;

        private int _factionNumber;
        private Button _button;
        private UnitGroup _unitGroup;
       // private FractionFrame _frame;
        private FractionTitle _title;

        public void Initialize(int number, List<UIUnit> units, DragAndDropController controller,Button button,CharacterSeller characterSeller)
        {
            _unitGroup = GetComponentInChildren<UnitGroup>();
       //     _frame=GetComponentInChildren<FractionFrame>();
            _title=GetComponentInChildren<FractionTitle>();
            _button=button;
            _factionNumber = number;

            InitializeUnitGroup(units, controller,characterSeller);

            if (_factionNumber != 0)
            {
                _unitGroup.gameObject.SetActive(false);
        //        _frame.gameObject.SetActive(false);
                _title.gameObject.SetActive(false);
                
            }
            else
                _button.interactable = false;
        }

        public void SetActive(bool isActive)
        {
            _button.interactable = !isActive;
            _unitGroup.gameObject.SetActive(isActive);
        //    _frame.gameObject.SetActive(isActive);
            _title.gameObject.SetActive(isActive);
        }

        private void InitializeUnitGroup(List<UIUnit> units, DragAndDropController controller,CharacterSeller characterSeller) => _unitGroup.Initialize(_factionNumber, units, controller, characterSeller);
    }
}