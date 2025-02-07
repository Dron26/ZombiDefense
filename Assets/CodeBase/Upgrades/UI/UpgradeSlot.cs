using Data.Upgrades;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeSlot: MonoCache
    {
        [SerializeField] private  Image _upgarderImage;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _arrowImage;
        [SerializeField] private Button _button;
        [SerializeField] private Button _lockButton;
        
        private UpgradeInfo _upgradeInfo;
        
        public int SlotIndex=> _slotIndex;
        private int _slotIndex;

        public Button GetUpgrateButton() => _button;
        public Button GetLockeButton() => _lockButton;
        public UpgradeInfo GetUpgradeData() => _upgradeInfo;
        
        public void Initialize(int slotIndex, UpgradeInfo upgradeInfo, bool isLocked)
        {
            _slotIndex=slotIndex;
            _upgradeInfo=upgradeInfo;
            IsLocked( isLocked);
        }
        
        public void IsLocked(bool isLocked)
        {
            _lockImage.gameObject.SetActive(isLocked);
            _button.interactable = !isLocked;
            _lockButton.interactable = isLocked;
            _arrowImage.gameObject.SetActive(!isLocked);
        }
    }
}