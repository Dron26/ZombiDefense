using System.Collections.Generic;
using Audio;
using EnemiesUI.AbstractEntity;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.PlatoonGenerator;
using Infrastructure.States;
using Service.ADS;
using Service.DragAndDrop;
using Service.SaveLoadService;
using UI.BuyAndMerge;
using UI.BuyAndMerge.Merge;
using UI.BuyAndMerge.Raid;
using UI.Fraction;
using UI.Resurse;
using UI.SceneSetArmy;
using UI.SettingsPanel;
using UI.Unit;
using UI.WarningWindow;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(UIMerge))]
    [DisallowMultipleComponent]
    public class MainScene : MonoCache
    {
        
        [SerializeField] private FractionsInitializer _fractionsInitializer;
        [SerializeField] private MergePanelInitializer _mergePanelInitializer;
        [SerializeField] private RaidInitializer _raidInitializer;
        [SerializeField] private Button _continueButton;
        [SerializeField] private List<UIUnit> _units;
        [SerializeField] private UIUnitEnemy _enemyUnit;
        [SerializeField] private DragAndDropController _controller;
        [SerializeField] private GameObject _slot;
        [SerializeField] private GameObject _raidSlot;
        [SerializeField] private UIMerge _merge;
        [SerializeField] private ArmyInitializer _armyInitializer;
            // [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private WindowSwither _windowSwither;
        [SerializeField] private ADCanvas _adCanvas;
        [SerializeField] private OpponentPlatoonGenerator _generator;
        [SerializeField] private UIMove _uiMove;
        [SerializeField] private CharacterSeller _characterSeller;
        [SerializeField] private LeaderboardPanel _leaderboardPanel;
        [SerializeField] private GameObject _loadingCurtainPrefab;
        [SerializeField] private SettingPanel _settingPanel;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private MergeUnitGroup _mergeUnitGroup;

        private YandexLeaderboard _yandexLeaderboard;
        private SaveLoad _saveLoad;
        private List<HumanoidUI> _playerPlatoon;
        private List<Enemy> _enemyPlatoon;
        private GameStateMachine _stateMachine;
        private GameObject _loadingCurtain  ;
        
        public void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            CreateLoadingCurtain();
            _yandexLeaderboard = FindObjectOfType<YandexLeaderboard>();
            _saveLoad=FindObjectOfType<SaveLoad>();
        }
        
        private void Start()
        {
         //   InitializeResursesCanvas();
            _controller.Initialize(_saveLoad, _adCanvas);
            _fractionsInitializer.Initialize(_units, _controller, _characterSeller);
            _mergePanelInitializer.Initialize(_controller, _slot,_saveLoad);
            _merge.Initialize(_units, _controller, _saveLoad, _mergePanelInitializer);
            _characterSeller.Initialize(_saveLoad, _adCanvas,_merge,_mergePanelInitializer.GetMergeUnitGroup(), _windowSwither);
            _raidInitializer.Initialize(_controller, _raidSlot);
            _continueButton.onClick.AddListener(ContinueButtonClicked);
            _yandexLeaderboard.Initialize(CreateLeaderboard());
            //_audioController.Initialize(_saveLoad);
            _settingPanel.Initialize(_audioManager, _saveLoad);
        }

        private void ContinueButtonClicked()
        {
            if (_raidInitializer.GetComponentInChildren<RaidUnitGroup>().IsPlatoonReady)
            {
                _controller.gameObject.SetActive(false);
                _armyInitializer.gameObject.SetActive(true);
                _armyInitializer.ClickButtonBack+=GoToFirstMenu;
                GetPlayerPlatoon();
                InitializeOpponentGenerator();
                GetEnemyPlatoon();
                InitializeArmys();
                SlideInWindows(1);
            }
            else
                _windowSwither.ShowWindow(1);
        }


        private void InitializeOpponentGenerator()
        {
            _generator.Initialize( _enemyUnit,_playerPlatoon);
        }
        
        private void InitializeArmys()
        {
             _armyInitializer.Initialize(_playerPlatoon,_enemyPlatoon, _saveLoad);
        }
        
        public void GetPlayerPlatoon()
        {
            _playerPlatoon = new List<HumanoidUI>();
            _playerPlatoon = _raidInitializer.GetComponentInChildren<RaidUnitGroup>().GetPlatoon();
        }
        
        public void GetEnemyPlatoon()
        {
            _enemyPlatoon=new List<Enemy>();
            _enemyPlatoon = _generator.GetPlatoon();
        }

        private LeaderboardPanel CreateLeaderboard()
        {
            GameObject panel = Instantiate(_leaderboardPanel.gameObject, _characterSeller.gameObject.transform, true);
            LeaderboardPanel leaderboardPanel = panel.GetComponentInChildren<LeaderboardPanel>();
            Canvas myCanvas = panel.GetComponent<Canvas>();

            myCanvas.worldCamera = FindObjectOfType<Camera>();
            leaderboardPanel.Initiallize();
            return leaderboardPanel;
        }
        
        private void GoToFirstMenu()
        {
            SlideInWindows(0);
            
            _leaderboardPanel.SetActiveButton();
        }
        
        private void SlideInWindows(int id)
        {
            _uiMove.SlideIn(id);
        }

        public void EnterBattleLevel()
        {
         //   _stateMachine.Enter<LoadLevelState,string>(SceneName.Game); 
            Destroy(gameObject);
        } 
        
        
      //  private void InitializeResursesCanvas() => _resursesCanvas.Initialize(_saveLoad, _characterSeller);


        protected override void  OnDisable()
        {
            if (_loadingCurtain!=null)
            {
         //       _loadingCurtain.GetComponent<LoadingCurtain>().OnFinishedShow -= GoToFirstMenu;
            }
            
            _armyInitializer.ClickButtonBack-=GoToFirstMenu;
        }

        private void  CreateLoadingCurtain()
        {
             _loadingCurtain = Instantiate(_loadingCurtainPrefab.gameObject);
    //        _loadingCurtain.GetComponent<LoadingCurtain>().OnFinishedShow += GoToFirstMenu;;
        }
    }
}