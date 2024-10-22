using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Data;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Lean.Localization;
using Service;
using Service.SaveLoad;
using Services.PauseService;
using TMPro;
using UI.HUD.StorePanel;
using UI.Levels;
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
        public Action OnClickNextLocation;
        public Action OnResetLevel;
        private GlobalTimer _globalTimer;
        private bool _isLastHumanoidDie;
        private GameStateMachine _stateMachine;
        private Wallet _wallet;
        private LocationManager _locationManager;
        private IPauseService _pauseService;
        public void Init(SaveLoadService saveLoadService , Store store,
            LocationManager locationManager)
        {
            _saveLoadService = saveLoadService;
            _locationManager = locationManager;
            _stateMachine = saveLoadService.GetGameBootstrapper().GetStateMachine();
            _panel.SetActive(false);
            _wallet=store.GetWallet();
            _pauseService = AllServices.Container.Single<IPauseService>();
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
            _pauseService.SetPause(isPaused);
        }
        private void SelectOk()
        {
            _panel.SetActive(false);
            _saveLoadService.ExitFromLocation(true);
            SetPaused(false);
            OnClickExitToMenu?.Invoke();
        }
        
        private void AddListener()
        {
            _backToMenu.onClick.AddListener(SwicthScene);
            _reset.onClick.AddListener(ResetLevel);
            _continue.onClick.AddListener(SelectOk);
            
            _saveLoadService.LastHumanoidDie+=OnLastHumanoidDie;
            _saveLoadService.OnSetCompletedLocation+=ShowReport;
        }

        private void RemoveListener()
        {
            _backToMenu.onClick.RemoveListener(SwicthScene);
            _reset.onClick.RemoveListener(ResetLevel);
            _continue.onClick.RemoveListener(SelectOk);
            
            _saveLoadService.LastHumanoidDie-=OnLastHumanoidDie;
            _saveLoadService.OnSetCompletedLocation-=ShowReport;
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