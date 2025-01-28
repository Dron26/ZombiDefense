using System;
using System.Collections;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using Services.SaveLoad;
using TMPro;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Logic.WaveManagment
{
    public class TimerDisplay : MonoCache
    { 
         public int zombiesRemainingToStartTimer = 1;
         public int _zombiesRemaining => _saveLoadService.Characters.ActiveCharacters.ToList().Count;
        [SerializeField] private int bonusCoinsPerSecond = 5;
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private GameObject _additionalPanel;
        [SerializeField] private TMP_Text _timeInfo;
        [SerializeField] private TMP_Text _bonusInfo;
        [SerializeField] private Button skipButton;
        public Action OnClickReady;

        private  SaveLoadService _saveLoadService;
        private Wallet _wallet;
        private string _nextWave = "The next wave";
        private string _bonus = "Bonus";
        private bool _isHumanoidReady=> _saveLoadService.GetActiveCharacters().Count > 0;
        private int bonusCoins;
        private bool _isStartClick = false;
         private float timerDuration ;
         private WaveManager _waveManager;

        public void Disabled()
        {
            RemoveListener();
        }

        public void Initialize(SaveLoadService saveLoadService, Wallet wallet, WaveManager waveManager)
        {
            _waveManager=waveManager;
             _saveLoadService = saveLoadService;
             _wallet=wallet;
          
            AddListener();
        }

        public void StartTimer()
        {
            _timerPanel.SetActive(true);
            _additionalPanel.SetActive(false);
            skipButton.gameObject.SetActive(true);
        }
        
        private void SetSpawnStarted()
        {
            if (_waveManager.CurrentStartedWave!=_waveManager.TotalWaves)
            {
                StartCoroutine(ShowSpawnTimer());
            }
        }
        
        private IEnumerator ShowSpawnTimer()
        {
            _isStartClick = false;
            timerDuration = _saveLoadService.Locations.TimeTimeBeforeNextWave;
            _timerPanel.SetActive(true);
            _additionalPanel.SetActive(true);
            skipButton.gameObject.SetActive(true);
            float startTime = Time.time;
            
            while (Time.time < startTime + timerDuration&&_isStartClick==false)
            {
                float timeRemaining = startTime + timerDuration - Time.time;
                bonusCoins = Mathf.FloorToInt(timeRemaining * bonusCoinsPerSecond);
                UpdateCountdownText(timeRemaining);

                yield return null;
            }

            SkipTimer();
        }
       
        private void SkipTimer()
        {
            if (_isHumanoidReady&&_isStartClick==false)
            {
                _isStartClick = true;
                FinishShowTimer();
            }
        }
        
        
        private void FinishShowTimer()
        {
            StopCoroutine(ShowSpawnTimer());
            _additionalPanel.SetActive(false);
            StartSpawn();
        }
        
        private void StartSpawn()
        {
            _timerPanel.SetActive(false);
            bonusCoins = Mathf.FloorToInt(timerDuration * bonusCoinsPerSecond);
            _wallet.AddMoney(bonusCoins);
            OnClickReady?.Invoke();
            Debug.Log("StartSpawn");
            _isStartClick = false;
        }

        private void UpdateCountdownText(float timeRemaining)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            _timeInfo.text=seconds.ToString();
            _bonusInfo.text=bonusCoins.ToString();
        }

        private void AddListener()
        {
            skipButton.onClick.AddListener(SkipTimer);
            _saveLoadService.LastEnemyRemained+=SetSpawnStarted;
        }

        private void RemoveListener()
        {
            skipButton.onClick.RemoveListener(SkipTimer);
            _saveLoadService.LastEnemyRemained-=SetSpawnStarted;
        }
    }
}                                              