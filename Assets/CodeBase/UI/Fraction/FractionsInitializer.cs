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
    public class FractionsInitializer : MonoCache
    {
        private List<UIUnit> _units = new();
        private List<Fraction> _factions = new(); 
        private DragAndDropController _controller; 
        private FractionButtonGroup _fractionButtonGroup; 
        private List<Button> _buttonGroup=new();
        private CharacterSeller _characterSeller;

        public void Initialize(List<UIUnit> units,DragAndDropController controller,CharacterSeller characterSeller)
        {
            _characterSeller = characterSeller;
            _units = units;
            _controller = controller;
            _fractionButtonGroup=transform.parent.GetComponentInChildren<FractionButtonGroup>();
            
            foreach (Transform button in _fractionButtonGroup.transform)
            {
                Button newButton = button.GetComponent<Button>();
                _buttonGroup.Add(newButton);
            }
            
            InitializateFactions();
        }

        public void OnClickFaction(Fraction fraction)
        {
            foreach (Fraction factionInList in _factions)
                factionInList.SetActive(factionInList.FactionNumber == fraction.FactionNumber);
        }

        private void InitializateFactions()
        {
            int number = 0;

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Fraction faction))
                {
                    faction.Initialize(number, GetUIUnitGroup(), _controller,_buttonGroup[number], _characterSeller);
                    _factions.Add(faction);
                    number++;
                }
            }
        }

        private List<UIUnit> GetUIUnitGroup()
        {
            List<UIUnit> _tempUnits = new List<UIUnit>();

            foreach (UIUnit unit in _units)
            {
                _tempUnits.Add(unit);
            }

            return _tempUnits;
        }
    }
}