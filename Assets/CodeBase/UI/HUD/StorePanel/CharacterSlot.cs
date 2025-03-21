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

        [SerializeField] private Image _lock;
        //[SerializeField] private  TMP_Text _name;
        public CharacterData Data => _data;
        public bool IsSelected => _isSelected;
        private  bool _isLock=false;
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
            _button.interactable = !_isLock;
            _lock.gameObject.SetActive(_isLock);
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

        public void SetLock(bool isLock)
        {
            _isLock=isLock;
        }
    }
}