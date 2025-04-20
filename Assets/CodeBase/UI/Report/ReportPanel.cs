using System;
using System.Collections;
using Common;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Lean.Localization;
using Services;
using Services.PauseService;
using Services.SaveLoad;
using TMPro;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Report
{
    public class ReportPanel:MonoCache
    {
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoSurvivalEnemies;
        [SerializeField] private TMP_Text _infoSurvivalEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoKilledEnemies;
        [SerializeField] private TMP_Text _infoKilledEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoProfit;
        [SerializeField] private TMP_Text _infoProfitValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoWaveBonus;
        [SerializeField] private TMP_Text _infoWaveBonusValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoDeadMercenary;
        [SerializeField] private TMP_Text _infoDeadMercenaryValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoOffer;
        [SerializeField] private TMP_Text _allProfit;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoAllProfit;
        [SerializeField] private Button _stayOnLocation;
        [SerializeField] private Button _backToMenu;
        [SerializeField] private Button _reset;
        [SerializeField] private Button _continue;
        
        [SerializeField] private GameObject _panel;
        
        private int _numberKilledEnemies;
        private int _numberSurvivalEnemies;
        private int _deadMercenary;
        private int _profit;
        public Action OnClickExitToMenu;
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
        public void Init(Store store,GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _panel.SetActive(false);
            _wallet=store.GetWallet();
            _pauseService = AllServices.Container.Single<IPauseService>();
            _achievementsHandler = AllServices.Container.Single<IAchievementsHandler>();
            _enemyHandler = AllServices.Container.Single<IEnemyHandler>();
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
        }

        public void ShowReport()
        {
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
            }
            else
            {
                _reset.transform.parent.gameObject.SetActive(false);
                _continue.transform.parent.gameObject.SetActive(true);
                _infoOffer.TranslationName = ReportKey.TasksCompleted.ToString();
            }

            _panel.SetActive(true);
            _numberKilledEnemies = _achievementsHandler.KilledEnemies;
            _deadMercenary = _achievementsHandler.DeadMercenaryCount;
            _profit = _wallet.MoneyForEnemy;
            _numberSurvivalEnemies=_enemyHandler.GetActiveEnemy().Count;
            
            if (_numberSurvivalEnemies!=0)
            {
                _infoSurvivalEnemies.TranslationName = ReportKey.SurvivorsEnemies.ToString();
                _infoSurvivalEnemiesValue.text = _numberSurvivalEnemies.ToString();  
            }
           
            _infoWaveBonus.TranslationName = ReportKey.Bonus.ToString();
            _infoWaveBonusValue.text = (_achievementsHandler.WaveComplatedCount * 100f).ToString();
            _infoDeadMercenary.TranslationName = ReportKey.Dead.ToString();
            _infoDeadMercenaryValue.text = _deadMercenary.ToString();
            _infoKilledEnemies.TranslationName = ReportKey.Killed.ToString();
            _infoKilledEnemiesValue.text = _numberKilledEnemies.ToString();
            _infoProfit.TranslationName = ReportKey.Profit.ToString();
            _infoProfitValue.text = _profit.ToString();
            
            
            _allProfit.text = _wallet.TempMoney+(_profit*0.1f).ToString();
           // _infoAllProfit.TranslationName = ReportKey.Profit.ToString();
        }

        private void SwicthScene()
        {
            _panel.SetActive(false);
            SetPaused(false);
            OnClickExitToMenu?.Invoke();
        }

        public void OnLastHumanoidDie()
        {
            _isLastHumanoidDie=true;
            ShowReport();
        }
     
        
        private void ResetLevel()
        {
            Debug.Log("ResetLevel()");
            SetPaused(false);
            OnResetLevel?.Invoke();
            _stateMachine.Enter<LoadLevelState,string>(Constants.Location); 
        }
        private void SetPaused(bool isPaused)
        {
            _pauseService.ChangePause(isPaused);
        }
        private void SelectOk()
        {
            _panel.SetActive(false);
            //_saveLoadService.ExitFromLocation(true);
            SetPaused(false);
            OnClickExitToMenu?.Invoke();
        }
        
        private void AddListener()
        {
            _backToMenu.onClick.AddListener(SwicthScene);
            _reset.onClick.AddListener(ResetLevel);
            _continue.onClick.AddListener(SelectOk);
            
            _eventBroadcaster.LastHumanoidDie+=OnLastHumanoidDie;
            _eventBroadcaster.OnLocationCompleted+=ShowReport;
        }

        private void RemoveListener()
        {
            _backToMenu.onClick.RemoveListener(SwicthScene);
            _reset.onClick.RemoveListener(ResetLevel);
            _continue.onClick.RemoveListener(SelectOk);
            
            _eventBroadcaster.LastHumanoidDie-=OnLastHumanoidDie;
            _eventBroadcaster.OnLocationCompleted-=ShowReport;
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
}