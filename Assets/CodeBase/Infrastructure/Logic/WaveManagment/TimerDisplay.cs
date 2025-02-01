using System;
using System.Collections;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
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
        [SerializeField] private int bonusCoinsPerSecond = 5;
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private GameObject _additionalPanel;
        [SerializeField] private TMP_Text _timeInfo;
        [SerializeField] private TMP_Text _bonusInfo;
        [SerializeField] private Button skipButton;
        public Action OnClickReady;

        private Wallet _wallet;
        private string _nextWave = "The next wave";
        private string _bonus = "Bonus";
        
        private int bonusCoins;
        private bool _isStartClick = false;
        private float timerDuration ;
        private WaveManager _waveManager;
        private IEnemyHandler _enemyHandler;
        private ILocationHandler _locationHandler;
        private IGameEventBroadcaster _eventBroadcaster;
        
        public void Disabled()
        {
            RemoveListener();
        }

        public void Initialize( Wallet wallet, WaveManager waveManager)
        {
            _waveManager=waveManager;
            _wallet=wallet;
            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _enemyHandler = AllServices.Container.Single<IEnemyHandler>();
            _locationHandler=AllServices.Container.Single<ILocationHandler>();
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
            timerDuration = _waveManager.WaveSpawner.TimeTimeBeforeNextWave;
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
            if (_enemyHandler.GetActiveEnemy().Count > 0&&_isStartClick==false)
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
            _eventBroadcaster.LastEnemyRemained+=SetSpawnStarted;
        }

        private void RemoveListener()
        {
            skipButton.onClick.RemoveListener(SkipTimer);
            _eventBroadcaster.LastEnemyRemained-=SetSpawnStarted;
        }
    }
}