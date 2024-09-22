using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class CharacterSlot : MonoCache
    {
        [SerializeField] private Image _icon;

        //[SerializeField] private  TMP_Text _name;
        public CharacterData Data => _data;
        public bool IsSelected => _isSelected;
        public int Price => _data.Price;
        public CharacterType Type => _data.Type;

        private CharacterData _data;
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


        public void Initialize(CharacterData data, CharacterStore store)
        {
            _store = store;
            _data = data;
            _icon.sprite = _data.Sprite;
        }
    }
}