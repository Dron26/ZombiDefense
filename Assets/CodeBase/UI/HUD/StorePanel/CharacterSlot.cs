using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class CharacterSlot : MonoCache
    {
        [SerializeField] private Image _icon;

        //[SerializeField] private  TMP_Text _name;
        public Character Character => _character;
        public bool IsSelected => _isSelected;
        public int Price => _character.Price;
        public int Index => _character.ID;

        private Character _character;
        private bool _isSelected = false;
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


        public void Initialize(Character character, CharacterStore store)
        {
            _store = store;
            _character = character;
            _icon.sprite = _character.Sprite;
        }
    }
}