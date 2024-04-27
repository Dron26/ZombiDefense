using System;
using System.Collections;
using Data;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Lean.Localization;
using Service.SaveLoad;
using TMPro;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Report
{
    public class ReportPanel:MonoCache
    {
        private SaveLoadService _saveLoadService;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoSurvivalEnemies;
        [SerializeField] private TMP_Text _infoSurvivalEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoKilledEnemies;
        [SerializeField] private TMP_Text _infoKilledEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoProfit;
        [SerializeField] private TMP_Text _infoProfitValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoSurvival;
        [SerializeField] private TMP_Text _infoSurvivalValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoDeadMercenary;
        [SerializeField] private TMP_Text _infoDeadMercenaryValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoOffer;

        [SerializeField] private Button _stayOnLocation;
        [SerializeField] private Button _backToMenu;
        [SerializeField] private Button _reset;
        [SerializeField] private Button _continue;
        
        [SerializeField] private GameObject _panel;
        
        private int _numberKilledEnemies;
        private int _allNumberKilledEnemies;
        private int _numberSurvivalEnemies;
        private int _survival;
        private int _deadMercenary;
        private int _profit;
        public Action OnClickExitToMenu;
        public Action OnStayInLication;
        public Action OnResetLevel;
        private GlobalTimer _globalTimer;
        private bool _isLastHumanoidDie;
        private GameStateMachine _stateMachine;
        private Wallet _wallet;
        
        public void Initialize(SaveLoadService saveLoadService, GlobalTimer globalTimer, Store store)
        {
            _saveLoadService = saveLoadService;
            _globalTimer=globalTimer;
            AddListener();
            _panel.SetActive(false);
            _stateMachine = saveLoadService.GetGameBootstrapper().GetStateMachine();
            _wallet=store.GetWallet();
        }

        public void ShowReport()
        {
            StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(2f);
            
            _globalTimer.SetPaused(true);
            
            if (_isLastHumanoidDie)
            {
                _stayOnLocation.gameObject.SetActive(false);
                _infoOffer.TranslationName = ReportKey.DeadOffer.ToString();
            }
            else
            {
                _stayOnLocation.gameObject.SetActive(true);
                _infoOffer.TranslationName = ReportKey.TasksCompleted.ToString();
            }

            Time.timeScale = 1;
            _panel.SetActive(true);
            _numberKilledEnemies = _saveLoadService.GetNumberKilledEnemies();
            _allNumberKilledEnemies = _saveLoadService.GetAllNumberKilledEnemies();
            _survival = _saveLoadService.GetSurvivalsCount();
            _deadMercenary = _saveLoadService.GetDeadMercenaryCount();
            _profit = _wallet.MoneyForEnemy;
            _numberSurvivalEnemies=_saveLoadService.GetActiveEnemy().Count;
            
            _infoSurvivalEnemies.TranslationName = ReportKey.SurvivorsEnemies.ToString();
            _infoSurvivalEnemiesValue.text = _numberSurvivalEnemies.ToString();
            _infoSurvival.TranslationName = ReportKey.Survivors.ToString();
            _infoSurvivalValue.text = _survival.ToString();
            _infoDeadMercenary.TranslationName = ReportKey.Dead.ToString();
            _infoDeadMercenaryValue.text = _deadMercenary.ToString();
            _infoKilledEnemies.TranslationName = ReportKey.Killed.ToString();
            _infoKilledEnemiesValue.text = _numberKilledEnemies.ToString();
            _infoProfit.TranslationName = ReportKey.Profit.ToString();
            _infoProfitValue.text = _profit.ToString();
        }

        private void SwicthScene()
        {
            _globalTimer.SetPaused(false);
            _panel.SetActive(false);
            OnClickExitToMenu?.Invoke();
        }

        public void OnLastHumanoidDie()
        {
            _isLastHumanoidDie=true;
            ShowReport();
        }
        
        private void StayInLocation()
        {
            _globalTimer.SetPaused(false);
            OnStayInLication?.Invoke();
            _panel.SetActive(false);
        }
        private void ResetLevel()
        {
            Debug.Log("ResetLevel()");
            OnResetLevel?.Invoke();
            _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Level); 
        }
        
        private void СontinueGame()
        {
            Debug.Log("Entered СontinueGame()");
            _globalTimer.SetPaused(false);
            OnStayInLication?.Invoke();
            _panel.SetActive(false);
        }
        
        private void AddListener()
        {
            _backToMenu.onClick.AddListener(SwicthScene);
            _reset.onClick.AddListener(ResetLevel);
            _continue.onClick.AddListener(СontinueGame);
            
            _saveLoadService.LastHumanoidDie+=OnLastHumanoidDie;
            _saveLoadService.OnCompleteLocation+=ShowReport;
        }

        private void RemoveListener()
        {
            _backToMenu.onClick.RemoveListener(SwicthScene);
            _reset.onClick.RemoveListener(ResetLevel);
            _continue.onClick.RemoveListener(СontinueGame);
            
            _saveLoadService.LastHumanoidDie-=OnLastHumanoidDie;
            _saveLoadService.OnCompleteLocation-=ShowReport;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
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
    TasksCompleted
}