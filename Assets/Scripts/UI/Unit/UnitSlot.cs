using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using UI.Empty;
using UnityEngine;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    public class UnitSlot : MonoCache
    {
        public int SequenceNumber => _sequenceNumber;
        public bool IsBusy => _isBusy;
        
        private CanvasGroup _canvasGroup;
        private bool _isBusy;
        private int _sequenceNumber;
        private EmptyUIUnit _empty;
        private DropObject _dropObject;
        private ParticleSystem _particleSystem;
        private void Awake()
        {
            _particleSystem=GetComponent<ParticleSystem>();
            _empty = GetComponentInChildren<EmptyUIUnit>();
            _canvasGroup=GetComponent<CanvasGroup>();
            _dropObject=GetComponent<DropObject>();
        }

        public void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
            _empty.gameObject.SetActive(true);
            
            if (isBusy)
            {
                if (_particleSystem != null) _particleSystem.Play();

                _canvasGroup.enabled =false;
                _dropObject.enabled =false;
                _empty.transform.SetSiblingIndex(1);
                _empty.gameObject.SetActive(false);
            }
            else
            {
                _canvasGroup.enabled =true;
                _dropObject.enabled =true;
                _empty.transform.SetSiblingIndex(0);
            }
            
            
        }

        public void SetNumber(int sequenceNumber) => _sequenceNumber = sequenceNumber;

        public Transform GetChildren() => transform.GetChild(0);
    }
}