using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using Service.SaveLoadService;
using UnityEngine;

namespace UI.BuyAndMerge.Merge
{
    [DisallowMultipleComponent]
    public class MergePanelInitializer : MonoCache
    {
        private List<GameObject> _slots = new();
        private DragAndDropController _controller;
        private GameObject _slot;
        private MergeUnitGroup _unitGroup;
        private const int _countUnit = 6;

        public void Initialize(DragAndDropController controller, GameObject slot ,SaveLoadService saveLoadService)
        {
            _controller = controller;
            _slot = slot;
            _unitGroup = GetComponentInChildren<MergeUnitGroup>();
            
            InitializeSlot();
            
            InitializateUiUnitGroup(saveLoadService);
        }

        public MergeUnitGroup GetMergeUnitGroup() => _unitGroup;
        
        private void InitializateUiUnitGroup(SaveLoadService saveLoadService) => _unitGroup.Initialize(_slots, _controller,saveLoadService);

        private void InitializeSlot()
        {
            for (int i = 0; i < _countUnit; i++)
            {
                _slots.Add(_slot);
            }
        }
    }
}