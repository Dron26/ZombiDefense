using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.DragAndDrop;
using TMPro;
using UI.Empty;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    public class UIUnit : MonoCache
    {
        public int FactionNumber => _factionNumber;
        public int PriorityNumber => _priorityNumber;
        public string Name => _name.text;
        
        private TMP_Text _name;
        private TMP_Text _price;
        private Image _image;
        private int _factionNumber;
        private int _priorityNumber;
        
        public void Initialize(int factionNumber, string name, int price, int priority)
        {
            _price = GetComponentInChildren<UnitPrice>().GetComponent<TMP_Text>();
            _name = GetComponentInChildren<UnitName>().GetComponent<TMP_Text>();
            
            if (transform.TryGetComponent(out UnitForBuy _))
            {
                    _image= GetComponentInChildren<UnitPrice>().GetComponentInChildren<Image>();
            }
            
            _factionNumber=factionNumber;
            
            if (price != 0)
            {
                _price.text = price.ToString();
            }

            _priorityNumber = priority;
            _name.text = name;
        }

        public void SetNameChildActive(bool isActive) => _name.enabled=isActive;
        
        public void SetPriceChildActive(bool isActive) => (_price.enabled, _image.enabled) = (isActive, isActive);

        public void OnIdle(bool isActive) => SetPriceChildActive( isActive);
    }
}