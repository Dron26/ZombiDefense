
    using System;
    using System.Collections;
    using Common;
    using Infrastructure.BaseMonoCache.Code.MonoCache;
    using Infrastructure.StateMachine;
    using Infrastructure.StateMachine.States;
    using Interface;
    using Lean.Localization;
    using Services;
    using Services.Analytic;
    using Services.PauseService;
    using TMPro;
    using UI;
    using UI.HUD.StorePanel;
    using UnityEngine;
    using UnityEngine.UI;

    public class ReportPanel : MonoCache
    {
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoKilledEnemies;
       [SerializeField] private TMP_Text _infoKilledEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoDeadMercenary;
        [SerializeField] private TMP_Text _infoDeadMercenaryValue;
        
        [SerializeField] private TMP_Text _bonusKilledEnemiesValue;
        [SerializeField] private TMP_Text _bonusDeadMercenaryValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoOffer;
        [SerializeField] private TMP_Text _allProfit;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoAllProfit;
        [SerializeField] private Button _stayOnLocation;
        [SerializeField] private Button _backToMenu;
        [SerializeField] private Button _reset;
        [SerializeField] private Button _continue;
        [SerializeField] private TMP_Text _bonusKilledEnemies;
        [SerializeField] private TMP_Text _bonusWaveBonusValue;
        [SerializeField] private GameObject _panel;

        private int _numberKilledEnemies;
        private int _numberSurvivalEnemies;
        private int _deadMercenary;
        public Action OnLocationPassed;
        public Action OnClickNextLocation;
        public Action OnResetLevel;
        private GlobalTimer _globalTimer;
        private bool _isLastHumanoidDie;
        private GameStateMachine _stateMachine;
        private Wallet _wallet;
        private IPauseService _pauseService;
        private IAchievementsHandler _achievementsHandler;
        private IEnemyHandler _enemyHandler;
        private IGameEventBroadcaster _eventBroadcaster;
        private IAnalyticService _analyticService;
        public void Init(Store store, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _panel.SetActive(false);
            _wallet = store.GetWallet();
            _pauseService = AllServices.Container.Single<IPauseService>();
            _achievementsHandler = AllServices.Container.Single<IAchievementsHandler>();
            _enemyHandler = AllServices.Container.Single<IEnemyHandler>();
            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _analyticService= AllServices.Container.Single<IAnalyticService>();

            AddListener();
        }

        public void ShowReport()
        {
            Debug.Log("ShowReport()");
            StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(4f);

            SetPaused(true);

            if (_isLastHumanoidDie)
            {
                _reset.transform.parent.gameObject.SetActive(true);
                _continue.transform.parent.gameObject.SetActive(false);
                _infoOffer.TranslationName = ReportKey.DeadOffer.ToString();
                _analyticService.LoseLevel();
            }
            else
            {
                _reset.transform.parent.gameObject.SetActive(false);
                _continue.transform.parent.gameObject.SetActive(true);
                _infoOffer.TranslationName = ReportKey.TasksCompleted.ToString();
                _analyticService.WinLevel();
            }

            _panel.SetActive(true);
            
            _infoKilledEnemies.TranslationName = ReportKey.Killed.ToString();
            _infoDeadMercenary.TranslationName = ReportKey.Dead.ToString();
             
            _numberKilledEnemies = _achievementsHandler.KilledEnemies;
            _infoKilledEnemiesValue.text = _numberKilledEnemies.ToString();
            _infoDeadMercenaryValue.text = _deadMercenary.ToString();
            _deadMercenary = _achievementsHandler.DeadMercenaryCount;
            
            _allProfit.text = "+  "+_wallet.GetAllProfit().ToString()+"  $";
        }

        private void SwicthScene()
        {
            _panel.SetActive(false);
            SetPaused(false);
            OnLocationPassed?.Invoke();
            
        }

        public void OnLastHumanoidDie()
        {
            _isLastHumanoidDie = true;
            ShowReport();
        }


        private void ResetLevel()
        {
            Debug.Log("ResetLevel()");
            SetPaused(false);
            OnResetLevel?.Invoke();
            _stateMachine.Enter<LoadLevelState, string>(Constants.Location);
        }

        private void SetPaused(bool isPaused)
        {
            _pauseService.ChangePause(isPaused);
        }

        private void SelectOk()
        {
            _panel.SetActive(false);
            SetPaused(false);
            OnLocationPassed?.Invoke();
        }

        private void AddListener()
        {
             _backToMenu.onClick.AddListener(SwicthScene);
             _continue.onClick.AddListener(SelectOk);

            _eventBroadcaster.LastHumanoidDie += OnLastHumanoidDie;
            _eventBroadcaster.OnLocationCompleted += ShowReport;
        }

        private void RemoveListener()
        {
             _continue.onClick.RemoveListener(SelectOk);

            _eventBroadcaster.LastHumanoidDie -= OnLastHumanoidDie;
            _eventBroadcaster.OnLocationCompleted -= ShowReport;
        }

        private void OnDestroy()
        {
            RemoveListener();
            _achievementsHandler.Reset();
        }
    }

    public enum ReportKey
    {
        SurvivorsEnemies,
        Survivors,
        Dead,
        Killed,
        Profit,
        DeadOffer,
        TasksCompleted,
        Bonus
    }
