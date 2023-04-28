using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UnityEngine;

namespace UI.BuyAndMerge.Raid
{
    [DisallowMultipleComponent]
    public class RaidCanvasInitializer : MonoCache
    {
        private List<GameObject> _slots = new();
        private DragAndDropController _controller;
        private GameObject _slot;
        private RaidUnitGroup _unitGroup;
        private const int _countUnit = 5;

        public void Initialize(DragAndDropController controller, GameObject slot)
        {
            _controller = controller;
            _slot = slot;
            _unitGroup = GetComponentInChildren<RaidUnitGroup>();
            
            InitializateUiUnit();
            InitializateUiUnitGroup();
        }

        private void InitializateUiUnitGroup() => _unitGroup.Initialize(_slots, _controller);

        private void InitializateUiUnit()
        {
            for (int i = 0; i < _countUnit; i++)
            {
                _slots.Add(_slot);
            }
        }
    }
}