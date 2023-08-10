using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class CloseButton:MonoCache
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _open;
        [SerializeField] private Image _close;

        private bool _isOpen=true;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _isOpen = !_isOpen;
            _open.gameObject.SetActive(_isOpen);
            _close.gameObject.SetActive(!_isOpen);
        }
    }
}