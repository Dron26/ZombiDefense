using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Upgrades;

namespace UI.SceneBattle.Store
{
    // public class StoreOnPlay : MonoCache
    // {
    //     [SerializeField] private CharacterStorePanel _characterStorePanel;
    //     [SerializeField] private Button _buttonSelectionPanel;
    //     [SerializeField] private Image _dimImage;
    //     [SerializeField] private Button _buttonRightPanel;
    //     [SerializeField] private WorkPointUpgradePanel _storePanel;
    //     [SerializeField] private GameObject _rightPanel;
    //     [SerializeField] private Wallet _wallet;
    //     [SerializeField] private List<int> _priceForWorkPointUp;
    //     [SerializeField] private WorkPointGroup _workPointGroup;
    //     [SerializeField] private UpgradGrouper _upgradGrouper;
    //     private bool isRightPanelOpen = true;
    //     private Button _closeButton;
    //     private WorkPoint _selectedWorkPoint;
    //     private List<Humanoid> _characters = new();
    //     private SceneInitializer _sceneInitializer;
    //     private PlayerCharacterInitializer _characterInitializer;
    //     private MovePointController _movePointController; 
    //     public UnityAction<Humanoid> BuyCharacter;
    //     private SaveLoad _saveLoad;
    //     private int maxLevel = 3;
    //
    //     
    //     public void Initialize(SceneInitializer initializer, SaveLoad saveLoad)
    //     {
    //         _saveLoad = saveLoad;
    //         _sceneInitializer = initializer;
    //         SetCharacterInitializer();
    //     }
    //
    //     private void SetCharacterInitializer()
    //     {
    //         _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
    //         //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
    //         _characters=_saveLoad.GetAvailableCharacters();
    //         _saveLoad.OnSelectedNewPoint += CheckPointInfo;
    //             // _storeCharacterStorePanel.Initialize(_characterInitializer, this);
    //         InitializeButton();
    //         _characterStorePanel.gameObject.SetActive(false);
    //         _characterStorePanel.BuyCharacter += OnBuyCharacter;
    //         _movePointController=_sceneInitializer.GetMovePointController();
    //         _storePanel.Initialize(_characterInitializer, _saveLoad);
    //         _storePanel.GetButton().onClick.AddListener(BuyPointUp);
    //         
    //         //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
    //         //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
    //         //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
    //     }
    //
    //     private void OnBuyCharacter(Humanoid humanoid)
    //     {
    //         BuyCharacter?.Invoke(humanoid);
    //         ClosePanel();
    //     }
    //
    //     private void InitializeButton()
    //     {
    //         _closeButton = _dimImage.GetComponent<Button>();
    //         _closeButton.onClick.AddListener(ClosePanel);
    //         _buttonSelectionPanel.onClick.AddListener(SetPanelInfoState);
    //         _buttonRightPanel.onClick.AddListener(ChangeStateRightPanel);
    //     }
    //
    //     private void CheckPointInfo(WorkPoint workPoint)
    //     {
    //         bool isStartPoint = false;
    //
    //         if (isStartPoint)
    //         {
    //             _selectedWorkPoint=workPoint;
    //
    //             if (_selectedWorkPoint.Level < _workPointGroup.MaxCountPrecent)
    //             {
    //                 _storePanel.ShowButton(true);
    //             }
    //             else
    //             {
    //                 _storePanel.ShowButton(false);
    //             }
    //         }
    //         else
    //         {
    //             isStartPoint = true;
    //         }
    //         
    //     }
    //
    //     private void BuyPointUp()
    //     {
    //         int price = _priceForWorkPointUp[_selectedWorkPoint.Level];
    //         
    //         if (_wallet.CheckPossibilityBuy(price))
    //         {
    //             _wallet.SpendMoney(price);
    //             _workPointGroup.UpLevel(_selectedWorkPoint);
    //         }
    //         else
    //         {
    //             print("должен мигать кошелек");
    //         }
    //     }
    //
    //     private void ShowPanel()
    //     {
    //         _characterStorePanel.gameObject.SetActive(true);
    //         _characterStorePanel.ShowAvaibleCharacters();
    //     }
    //
    //     private void ClosePanel()
    //     {
    //         _dimImage.gameObject.SetActive(false);
    //         _characterStorePanel.gameObject.SetActive(false);
    //     }
    //
    //     public List<Humanoid> GetAvaibleCharacters()
    //     {
    //         return _characters;
    //     }
    //
    //     public CharacterStorePanel GetBuyedPanel()
    //     {
    //         return _characterStorePanel;
    //     }
    //
    //     public void SetButtonState(bool isActive)
    //     {
    //         _buttonSelectionPanel.gameObject.SetActive(isActive);
    //     }
    //
    //     public void SetPanelInfoState()
    //     {
    //         if (_characterStorePanel.gameObject.activeSelf)
    //         {
    //             _characterStorePanel.gameObject.SetActive( false);
    //             _dimImage.gameObject.SetActive( false);
    //             _storePanel.gameObject.SetActive( false);
    //         }
    //         else
    //         {
    //             _characterStorePanel.gameObject.SetActive( true);
    //             _dimImage.gameObject.SetActive( true);
    //             _storePanel.gameObject.SetActive( true);
    //         }
    //     }
    //
    //     private void ChangeStateRightPanel()
    //     {
    //         isRightPanelOpen=!isRightPanelOpen;
    //         _rightPanel.gameObject.SetActive(isRightPanelOpen);
    //     }
    //
    //     private void Buy(int price)
    //     {
    //
    //         
    //     }
    //}
}