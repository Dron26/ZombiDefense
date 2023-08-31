using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Logic.WaveManagment
{
    public class TimerDisplay : MonoCache
    { 
         public int zombiesRemainingToStartTimer = 2;
        public int _zombiesRemaining => _saveLoadService.GetActiveEnemy().Count;
        [SerializeField] public float timerDuration = 10f;
        [SerializeField] public int bonusCoinsPerSecond = 5;
        [SerializeField] public GameObject _timerPanel;
        [SerializeField] public TMP_Text countdownText;
        [SerializeField] public Button skipButton;
private bool _isHumanoidReady=> _saveLoadService.GetActiveHumanoids().Count > 0;
        private bool hasSpawnStarted = false;
        private int bonusCoins;
        private  SaveLoadService _saveLoadService;
        private bool _isStartClick = true;
        public Action OnClickStartSpawn;
        
        private void Start()
        {
            countdownText.text = "";
            _timerPanel.SetActive(true);
            skipButton.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(false);
        }
        protected override void OnEnabled()
        {
            skipButton.onClick.AddListener(SkipTimer);
        }

        protected override void OnDisabled()
        {
            skipButton.onClick.RemoveListener(SkipTimer);
        }

        public void StartTimer(SaveLoadService saveLoadService)
        {
             _saveLoadService = saveLoadService;
            StartCoroutine(CheckZombieCountAndSpawn());
        }

        private IEnumerator CheckZombieCountAndSpawn()
        {
            while (true)
            {
                if (zombiesRemainingToStartTimer <= _zombiesRemaining && !hasSpawnStarted)
                {
                    StartCoroutine(ShowSpawnTimer());
                }

                yield return new WaitForSeconds(2f);
            }
        }
        
        private IEnumerator ShowSpawnTimer()
        {
            countdownText.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            float startTime = Time.time;
            
            while (Time.time < startTime + timerDuration)
            {
                float timeRemaining = startTime + timerDuration - Time.time;
                bonusCoins = Mathf.FloorToInt(timeRemaining * bonusCoinsPerSecond);
                UpdateCountdownText(timeRemaining);

                yield return null;
            }

            FinishShowTimer();
        }
       
        private void SkipTimer()
        {
            if (!_isHumanoidReady) return;
            if (_isStartClick==false)
            {
                StopCoroutine(ShowSpawnTimer());
                FinishShowTimer();
            }
            else
            {
                _isStartClick = !_isStartClick;
                StartSpawn();
            }

        }
        
        private void FinishShowTimer()
        {
            countdownText.text = "";
            StartSpawn();
        }
        
        private void StartSpawn()
        {
            _timerPanel.SetActive(false);
            hasSpawnStarted = true;
            bonusCoins = Mathf.FloorToInt(timerDuration * bonusCoinsPerSecond);
            OnClickStartSpawn?.Invoke();
            Debug.Log("StartSpawn");
        }

        private void UpdateCountdownText(float timeRemaining)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            countdownText.text = "Spawn in: " + seconds + "s\nBonus Coins: " + bonusCoins;
        }
    }
}
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        // [SerializeField] private WaveManager _waveManager;
        // [SerializeField] private Button _timerButton;
        // [SerializeField] private TMP_Text _timerText;
        // [SerializeField] private int _maxCoins = 100;
        // [SerializeField] private Button _buttonStartSpawning;
        //
        // private bool _isWaiting;
        // private float _currentTimer;
        // private float _totalTime;
        //
        // public void Initialize(PlayerCharacterInitializer playerInitial)
        // {
        //     playerInitial.CreatedHumanoid += OnCreatedHumanoid;
        //     _timerButton.onClick.AddListener(OnTimerButtonClicked);
        //     _timerButton.gameObject.SetActive(false);
        //     _timerText.gameObject.SetActive(false);
        //
        //     // _waveManager.SpawningCompleted += OnSpawningCompleted;
        //     _buttonStartSpawning.onClick.AddListener(Spawn);
        //     _buttonStartSpawning.interactable = false;
        // }
        //
        //
        // private void OnCreatedHumanoid()
        // {
        //     _buttonStartSpawning.interactable = true;
        // }
        //
        // private void OnSpawningCompleted()
        // {
        //     _isWaiting = true;
        //     _totalTime = _waveManager.TimeBetweenWaves;
        //     _currentTimer = _totalTime;
        //
        //     _timerButton.gameObject.SetActive(true);
        //     _timerText.gameObject.SetActive(true);
        //
        //     StartCoroutine(CountdownTimer());
        // }
        //
        // private IEnumerator CountdownTimer()
        // {
        //     while (_isWaiting && _currentTimer > 0f)
        //     {
        //         _currentTimer -= Time.deltaTime;
        //         UpdateTimerText();
        //         yield return null;
        //     }
        //
        //     if (_isWaiting)
        //     {
        //         _waveManager.StartSpawn();
        //         _timerButton.gameObject.SetActive(false);
        //         _timerText.gameObject.SetActive(false);
        //     }
        // }
        //
        // private void UpdateTimerText()
        // {
        //     int minutes = Mathf.FloorToInt(_currentTimer / 60f);
        //     int seconds = Mathf.FloorToInt(_currentTimer % 60f);
        //     _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        // }
        //
        // private void OnTimerButtonClicked()
        // {
        //     _isWaiting = false;
        //
        //     int coins = CalculateCoins();
        //     // Выполните действие с монетами, например, добавьте их к игроку
        //
        //     Debug.Log("Coins earned: " + coins);
        //
        //     Spawn();
        //     _timerButton.gameObject.SetActive(false);
        //     _timerText.gameObject.SetActive(false);
        // }
        //
        // private int CalculateCoins()
        // {
        //     float percentage = 1f - (_currentTimer / _totalTime);
        //     int coins = Mathf.RoundToInt(percentage * _maxCoins);
        //     return coins;
        // }
        //
        // private void Spawn()
        // {
        //     _buttonStartSpawning.gameObject.SetActive(false);
        //     _waveManager.StartSpawn();
        // }
    
    //}
//}
