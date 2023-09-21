using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Report
{
    public class ReportPanel:MonoCache
    {
        private SaveLoadService _saveLoadService;
        
        [SerializeField] private TMP_Text _infoKilledEnemies;
        [SerializeField] private TMP_Text _infoKilledEnemiesValue;
        [SerializeField] private TMP_Text _infoProfit;
        [SerializeField] private TMP_Text _infoProfitValue;
        [SerializeField] private TMP_Text _infoSurvival;
        [SerializeField] private TMP_Text _infoSurvivalValue;
        [SerializeField] private TMP_Text _infoDeadMercenary;
        [SerializeField] private TMP_Text _infoDeadMercenaryValue;

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

        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _buttonApply.onClick.AddListener(СontinueGame);
            _buttonExit.onClick.AddListener(SwicthScene);
            _panel.SetActive(false);
        }

        private void СontinueGame()
        {
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
            _panel.SetActive(true);
            _numberKilledEnemies = _saveLoadService.GetNumberKilledEnemies();
            _allNumberKilledEnemies = _saveLoadService.GetAllNumberKilledEnemies();
            _survival = _saveLoadService.GetSurvivalsCount();
            _deadMercenary = _saveLoadService.GetDeadMercenaryCount();
            _profit = _saveLoadService.MoneyData.MoneyForEnemy;
            
            _infoSurvival.text = ReportKey.Survival.ToString();
            _infoSurvivalValue.text = _survival.ToString();
            _infoDeadMercenary.text = ReportKey.DeadMercenary.ToString();
            _infoDeadMercenaryValue.text = _deadMercenary.ToString();
            _infoKilledEnemies.text = ReportKey.KilledEnemies.ToString();
            _infoKilledEnemiesValue.text = _numberKilledEnemies.ToString();
            _infoProfit.text = ReportKey.Profit.ToString();
            _infoProfitValue.text = _profit.ToString();
        }

        private void SwicthScene()
        {
            _panel.SetActive(false);
            OnClickExitToMenu?.Invoke();
        }
        
        
    }
}

public enum ReportKey
{
    Survival,
    DeadMercenary,
    KilledEnemies,
    Profit
}