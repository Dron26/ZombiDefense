using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ButtonPanel : MonoCache
    {
        [SerializeField] private AdditionalWeaponButton _additionalWeapon;
        [SerializeField] private GameObject _rightPanel;
        [SerializeField] private GameObject _leftPanel;
        [SerializeField] private GameObject _enemyCountPanel;
        [SerializeField] private GameObject _additionStore;
        [SerializeField] private GameObject _additionPanel;
        [SerializeField] private GameObject _downPanel;
        [SerializeField] private Button _buttonRightPanel;

        private CountEnemyPanel _countEnemyPanel;
        private bool isActive = true;
        private bool isButtonPanelOpen = true;

        public void Initialize()
        {
            _countEnemyPanel = _enemyCountPanel.GetComponentInChildren<CountEnemyPanel>();
            _countEnemyPanel.Initialize();
            _additionalWeapon.Initialize();
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);

        }

        public void SwitchPanelState()
        {
            isActive = !isActive;
            _rightPanel.SetActive(isActive);
            _leftPanel.SetActive(isActive);
            _enemyCountPanel.gameObject.SetActive(isActive);
            _additionStore.SetActive(isActive);
            _additionPanel.SetActive(isActive);
        }
        
        private void ChangeStateButtonPanel()
        {
            isButtonPanelOpen = !isButtonPanelOpen;
            _additionPanel.gameObject.SetActive(isButtonPanelOpen);
            _downPanel.gameObject.SetActive(isButtonPanelOpen);
        }
    }
}