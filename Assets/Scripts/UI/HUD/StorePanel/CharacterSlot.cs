using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class CharacterSlot: MonoCache
    {
       [SerializeField] private Image _icon;
       [SerializeField] private  TMP_Text _name;
        public Humanoid Humanoid => _humanoid;
        public bool IsSelected => _isSelected;
        public int Price => _humanoid.GetPrice();
        public int Index => _humanoid.ID;

        private Humanoid _humanoid;
        private bool _isSelected=false;
        private Button _button;
        private CharacterStore _store;

        public UnityAction<CharacterSlot> Selected;
        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            Selected?.Invoke(this);
        }
        

        public void Initialize( Humanoid humanoid,CharacterStore store)
        {
            _store=store;
            _humanoid = humanoid;
            _icon.sprite = _humanoid.GetSprite();
            _name.text = humanoid.GetName();
        }
        
    }
}