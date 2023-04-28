using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UI.Empty;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Unit
{ 
    [DisallowMultipleComponent]
    public class UIUnitEnemy : MonoCache
    {
        public string Name => _name.text;
        [SerializeField] private List<Sprite> _icons = new();
        [SerializeField] private Image _icon;
        private TMP_Text _name;
        
        public void Initialize(string name)
        {
            _name = GetComponentInChildren<UnitName>().GetComponent<TMP_Text>();
            _name.text = name;

            foreach (Sprite icon in _icons)
            {
                if (icon.name==name)
                {
                    _icon.sprite = icon;
                }
            }
        }

        public void SetNameChildActive(bool isActive) => _name.enabled=isActive;
    }
}