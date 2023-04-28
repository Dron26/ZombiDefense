using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.Empty;
using UnityEngine;

namespace UI.SceneSetArmy.Slots
{
    [DisallowMultipleComponent]
    public class ParametrSlot : MonoCache
    {

        public int SequenceNumber => _sequenceNumber;
        public bool IsBusy => _isBusy;
        private CanvasGroup _canvasGroup;
        private bool _isBusy;
        private int _sequenceNumber;
        private EmptyUIUnit _empty;
        public int NumberOfUnit => _numberOfUnit;
        private int _numberOfUnit;

        public void  Initialize()
        {
            _empty = GetComponentInChildren<EmptyUIUnit>();
            _canvasGroup=GetComponent<CanvasGroup>();
            
            SetBusy(true);
        }

        public void AddQuantity(int quantity) => _numberOfUnit = quantity;

        private void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
            
            _empty.gameObject.SetActive(true);
            
            if (isBusy)
            {
                _canvasGroup.enabled =false;
                _empty.transform.SetSiblingIndex(1);
                _empty.gameObject.SetActive(false);
            }
            else
            {
                _canvasGroup.enabled =true;
                _empty.transform.SetSiblingIndex(0);
            }
        }
    }
}
