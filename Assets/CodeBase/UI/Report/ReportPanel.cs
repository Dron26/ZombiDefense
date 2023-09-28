using System;
using System.Collections;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Lean.Localization;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Report
{
    public class ReportPanel:MonoCache
    {
        private SaveLoadService _saveLoadService;
        
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoKilledEnemies;
        [SerializeField] private TMP_Text _infoKilledEnemiesValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoProfit;
        [SerializeField] private TMP_Text _infoProfitValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoSurvival;
        [SerializeField] private TMP_Text _infoSurvivalValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoDeadMercenary;
        [SerializeField] private TMP_Text _infoDeadMercenaryValue;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _infoOffer;

        [SerializeField] private Button _buttonApply;
        [SerializeField] private Button _buttonExit;
        
        [SerializeField] private GameObject _panel;
        
        private int _numberKilledEnemies;
        private int _allNumberKilledEnemies;
        private int _survival;
        private int _deadMercenary;
        private int _profit;
        public Action OnClickExitToMenu;
        public Action OnClickContinue;
        private TimeManager _timeManager;
        private bool _isLastHumanoidDie;

        public void Initialize(SaveLoadService saveLoadService, TimeManager timeManager)
        {
            _saveLoadService = saveLoadService;
            _timeManager=timeManager;
            _buttonApply.GetComponentInChildren<Button>().onClick.AddListener(СontinueGame);
            _buttonExit.onClick.AddListener(SwicthScene);
            _panel.SetActive(false);
        }

        private void СontinueGame()
        {
            _timeManager.SetPaused(false);
            OnClickContinue?.Invoke();
            _panel.SetActive(false);
            Debug.Log("Entered СontinueGame()");
        }

        public void ShowReport()
        {
            
            StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(2f);
            
            _timeManager.SetPaused(true);
            
            if (_isLastHumanoidDie)
            {
                _buttonApply.gameObject.SetActive(false);
                _infoOffer.TranslationName = ReportKey.DeadOffer.ToString();
            }
            else
            {
                _buttonApply.gameObject.SetActive(true);
                _infoOffer.TranslationName = ReportKey.TasksCompleted.ToString();
            }

            Time.timeScale = 1;
            _panel.SetActive(true);
            _numberKilledEnemies = _saveLoadService.GetNumberKilledEnemies();
            _allNumberKilledEnemies = _saveLoadService.GetAllNumberKilledEnemies();
            _survival = _saveLoadService.GetSurvivalsCount();
            _deadMercenary = _saveLoadService.GetDeadMercenaryCount();
            _profit = _saveLoadService.MoneyData.MoneyForEnemy;
            
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
            _timeManager.SetPaused(false);
            _panel.SetActive(false);
            OnClickExitToMenu?.Invoke();
        }


        public void OnLastHumanoidDie()
        {
            _isLastHumanoidDie=true;
            ShowReport();
        }
        
    }
}

public enum ReportKey
{
    Survivors,
    Dead,
    Killed,
    Profit,
    DeadOffer,
    TasksCompleted
}