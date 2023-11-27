using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ButtonPanel : MonoCache
    {
        [SerializeField] private Button _menu;
        [SerializeField] private AdditionalWeaponButton _additionalWeapon;
        [SerializeField] private GameObject _rightPanel;
        [SerializeField] private GameObject _leftPanel;
        [SerializeField] private GameObject _enemyCountPanel;
        [SerializeField] private GameObject _additionStore;
        [SerializeField] private GameObject _additionPanel;
            
        private CountEnemyPanel _countEnemyPanel;
        private bool isActive = true;

        public void Initialize(SaveLoadService saveLoadService)
        {
            _countEnemyPanel = _enemyCountPanel.GetComponentInChildren<CountEnemyPanel>();
            _countEnemyPanel.Initialize(saveLoadService);
            _additionalWeapon.Initialize(saveLoadService);
            InitializeButton();
        }

        private void InitializeButton()
        {
            _menu.onClick.AddListener(SwitchPanelState);
           
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
    }
}