using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Locations
{
    public class LocationUIElement : MonoCache
    {
        public int Id => _id;
        public bool IsTutorial => _isTutorial;
        public bool IsLocked => _isLocked;
        public bool IsCompleted => _isCompleted;

        [SerializeField] private int _id;
        [SerializeField] private bool _isTutorial;
        [SerializeField] private bool _isLocked;
        [SerializeField] private Image _locked;
        [SerializeField] private Image _unlocked;
        private bool _isCompleted;
        private Button _button;

        public void SetLock(bool isLocked)
        {
            _button=GetComponentInChildren<Button>();
            _isLocked = isLocked;
            _locked.enabled=isLocked;
            _unlocked.enabled=!isLocked;
            _button.interactable=!isLocked;
            
            _button=GetComponent<Button>();

            _button.image = isLocked ? _locked : _unlocked;
        }

        public void SetCompleted(bool isCompleted)
        {
            _isCompleted=isCompleted;
        }

        public void Initialize( int id, bool isTutorial, bool isLocked,bool isCompleted )
        {
            _id = id;
            _isTutorial = isTutorial;
            _isLocked = isLocked;
            _isCompleted = isCompleted;
        }
    }
}