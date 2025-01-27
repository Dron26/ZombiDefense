    using System;
    using Infrastructure.BaseMonoCache.Code.MonoCache;
    using UI.Locations;
    using UnityEngine;
    using UnityEngine.UI;

    public class LocationUIElement : MonoCache
    {
        [SerializeField] private int _id;
        [SerializeField] private Image _lockedImage;
        [SerializeField] private Image _unlockedImage;
        [SerializeField] private Image _selectImage;

        private Button _button;
        public int Id => _id;
        public event Action<int> OnClick;
        private bool _isLocked;
        private bool _isCompleted;

        public void Initialize(bool isLocked, bool isCompleted)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick?.Invoke(_id));
            _isLocked=isLocked;
            _isCompleted=isCompleted;
            UpdateUI();
        }

        public void UpdateUI()
        {
            _lockedImage.enabled = _isLocked;
            _unlockedImage.enabled = !_isLocked;
            _selectImage.enabled = _isCompleted;
            _button.interactable = _isCompleted;
        }
    }