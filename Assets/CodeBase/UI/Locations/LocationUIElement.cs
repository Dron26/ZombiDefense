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

        public void Initialize(Location location)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick?.Invoke(_id));
            UpdateUI(location);
        }

        public void UpdateUI(Location location)
        {
            _lockedImage.enabled = location.IsLocked;
            _unlockedImage.enabled = !location.IsLocked;
            _selectImage.enabled = location.IsCompleted;
            _button.interactable = !location.IsLocked;
        }
    }