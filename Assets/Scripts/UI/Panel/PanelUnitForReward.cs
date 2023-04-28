using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UI.BuyAndMerge;
using UI.BuyAndMerge.Merge;
using UI.Empty;
using UI.WarningWindow;
using UnityEngine;

namespace UI.Panel
{
    [DisallowMultipleComponent]
    public class PanelUnitForReward : MonoCache
    {
        [SerializeField] private UIMerge _uiMerge;
        [SerializeField] private MergeUnitGroup _unitGroup;
        [SerializeField] private WindowSwither _windowSwither;
        [SerializeField] private CharacterSeller _characterSeller;
        

        private PanelProposal _panelProposal;
        private bool isCanView;

        private void Awake()
        {
            _panelProposal = GetComponentInChildren<PanelProposal>();
            _panelProposal.gameObject.SetActive(false);
        }

        public void GetReward() => _uiMerge.SetUnit(_characterSeller.GetAdViwer());

        public void SelectPanel()
        {
            if (CanShowAd())
            {
                _panelProposal.gameObject.SetActive(true);
            }
            else
            {
                _windowSwither.ShowWindow(0);
            }

            isCanView = false;
        }

        private bool CanShowAd()
        {
            if (_unitGroup.HaveEmptySlot())
            {
                isCanView = true;
            }

            return isCanView;
        }
    }
}