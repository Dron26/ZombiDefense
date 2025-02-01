using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Locations
{
    public class LocationUIElement : MonoCache
    {
        [SerializeField] private int _id;
        [SerializeField] private Image _lockedImage;
        [SerializeField] private Image _unlockedImage;
        [SerializeField] private Image _selectImage;
        [SerializeField] private Sprite _highlightedSprite;
        [SerializeField] private Image _targetImage;

        private Button _button;
        public int Id => _id;
        public event Action<int> OnClick;
        private bool _isLocked;
        private bool _isCompleted;

        public void Initialize(bool isLocked, bool isCompleted)
        {
            _isLocked=isLocked;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick?.Invoke(_id));
            _button.transition = Selectable.Transition.SpriteSwap;

            _isCompleted = isCompleted;
            UpdateState();
        }

        public void UpdateState()
        {
          
            
             _lockedImage.enabled = _isLocked;
             _unlockedImage.enabled = !_isLocked;
             _button.interactable = !_isLocked;
            
            if (_isLocked)
            {
                _button.targetGraphic = _lockedImage;
            }
            else
            {
                _button.targetGraphic = _unlockedImage;
            }

        }

        private void UpdateTransition()
        {
            

           
        }

        public void SetSelected(bool isSelected)
        {
            _isLocked = isSelected;
        }
    }
}