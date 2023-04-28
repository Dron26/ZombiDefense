using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using HumanoidsUI.Cyber;
using HumanoidsUI.Heroes;
using HumanoidsUI.People;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UI.BuyAndMerge;
using UnityEngine;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    
    public class UnitGroup : MonoCache
    {
        private List<UIUnit> _units = new();
        private List<GameObject> _factionUits = new();
        private int _factionNumber;
        private DragAndDropController _controller;

        private TempUIUnit _tempUIUnit;

        private bool _isActive;
        private CharacterSeller _characterSeller;

        public void Initialize(int number, List<UIUnit> units, DragAndDropController controller,CharacterSeller characterSeller)
        {
            _characterSeller = characterSeller;
            _factionNumber = number;
            _controller = controller;
            _tempUIUnit = GetComponentInChildren<TempUIUnit>();
            _tempUIUnit.Initialize();
            _tempUIUnit.gameObject.SetActive(false);

            TakeUIUnitComponent(units);
            SetUnitToSlot();
        }

        public void SetEmptySlot(int number)
        {
            _isActive = !_isActive;
            int defaultIndex = 5;
            int index;

            index = _isActive ? number : defaultIndex;
            _tempUIUnit.transform.SetSiblingIndex(index);
            _tempUIUnit.gameObject.SetActive(_isActive);
        }

        private void TakeUIUnitComponent(List<UIUnit> units)
        {
            Transform unitTransform;
            int priorityFirst = 0;
            int prioritySecond = 1;
            int priorityThird = 2;
            int priorityFourth = 3;
            
            foreach (UIUnit unit in units)
            {
                unitTransform = unit.transform;

                if (unitTransform.TryGetComponent(out Soldier soldier) && _factionNumber == 0)
                {
                    InitializeUIUnit(_factionNumber, soldier.name, soldier.GetPrice(), unit, priorityFirst);
                }
                else if (unitTransform.TryGetComponent(out Archer archer) && _factionNumber == 0)
                {
                    InitializeUIUnit(_factionNumber, archer.name, archer.GetPrice(), unit, prioritySecond);
                }
                else if (unitTransform.TryGetComponent(out Knight knight) && _factionNumber == 0)
                {
                    InitializeUIUnit(_factionNumber, knight.name, knight.GetPrice(), unit, priorityThird);
                }
                else if (unitTransform.TryGetComponent(out King king) && _factionNumber == 0)
                {
                    InitializeUIUnit(_factionNumber, king.name, king.GetPrice(), unit, priorityFourth);
                }
                else if (unitTransform.TryGetComponent(out CyberSoldier soldierCyber) && _factionNumber == 1)
                {
                    InitializeUIUnit(_factionNumber, soldierCyber.name, soldierCyber.GetPrice(), unit, priorityFirst);
                }
                else if (unitTransform.TryGetComponent(out CyberArcher archerCyber) && _factionNumber == 1)
                {
                    InitializeUIUnit(_factionNumber, archerCyber.name, archerCyber.GetPrice(), unit, prioritySecond);
                }
                else if (unitTransform.TryGetComponent(out CyberKnight knightCyber) && _factionNumber == 1)
                {
                    InitializeUIUnit(_factionNumber, knightCyber.name, knightCyber.GetPrice(), unit, priorityThird);
                }
                else if (unitTransform.TryGetComponent(out CyberKing kingCyber) && _factionNumber == 1)
                {
                    InitializeUIUnit(_factionNumber, kingCyber.name, kingCyber.GetPrice(), unit, priorityFourth);
                }
                else if (unitTransform.TryGetComponent(out CrazyTractor tractor) && _factionNumber == 2)
                {
                    InitializeUIUnit(_factionNumber, tractor.name, 0, unit, priorityFirst);
                }
                else if (unitTransform.TryGetComponent(out CyberZombie zombie) && _factionNumber == 2)
                {
                    InitializeUIUnit(_factionNumber, zombie.name, 0, unit, prioritySecond);
                }
                else if (unitTransform.TryGetComponent(out GunGrandmother grandmother) && _factionNumber == 2)
                {
                    InitializeUIUnit(_factionNumber, grandmother.name, 0, unit, priorityThird);
                }
                else if (unitTransform.TryGetComponent(out Virus virus) && _factionNumber == 2)
                {
                    InitializeUIUnit(_factionNumber, virus.name, 0, unit, priorityFourth);
                }
            }
        }

        private void InitializeUIUnit(int number, string name, int price, UIUnit unit, int priority)
        {
            unit.Initialize(number, name, price, priority);
            _units.Add(unit);
        }

        private void SetUnitToSlot()
        {
            int priorityHeroes = 2;

            for (int i = 0; i < _units.Count; i++)
            {
                foreach (UIUnit unit in _units)
                {
                    if (unit.PriorityNumber == i)
                    {
                        GameObject newUnit = Instantiate(unit.gameObject, transform);

                        HumanoidUI humanoid = unit.GetComponent<HumanoidUI>();
                        int price = unit.FactionNumber != priorityHeroes ? humanoid.GetPrice() : 0;

                        newUnit.GetComponent<UIUnit>()
                            .Initialize(unit.FactionNumber, unit.Name, price, unit.PriorityNumber);

                        newUnit.GetComponent<UnitForBuy>().Initialize(_characterSeller);
                        
                        
                        _factionUits.Add(newUnit);
                    }
                }
            }
        }
    }
}