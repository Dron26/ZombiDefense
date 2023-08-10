using System.Collections;
using Infrastructure.Location;
using Infrastructure.WaveManagment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private Button _timerButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private int _maxCoins = 100;
    [SerializeField] private Button _buttonStartSpawning;
    
    private bool _isWaiting;
    private float _currentTimer;
    private float _totalTime;
    
    public void Initialize(PlayerCharacterInitializer playerInitial)
    {
        playerInitial.CreatedHumanoid += OnCreatedHumanoid;
        _timerButton.onClick.AddListener(OnTimerButtonClicked);
        _timerButton.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
        
       // _waveManager.SpawningCompleted += OnSpawningCompleted;
        _buttonStartSpawning.onClick.AddListener(Spawn);
        _buttonStartSpawning.interactable = false;
    }

    
    private void OnCreatedHumanoid()
    {
        _buttonStartSpawning.interactable = true;
    }
    
    private void OnSpawningCompleted()
    {
        _isWaiting = true;
        _totalTime = _waveManager.TimeBetweenWaves;
        _currentTimer = _totalTime;
        
        _timerButton.gameObject.SetActive(true);
        _timerText.gameObject.SetActive(true);
        
        StartCoroutine(CountdownTimer());
    }
    
    private IEnumerator CountdownTimer()
    {
        while (_isWaiting && _currentTimer > 0f)
        {
            _currentTimer -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }
        
        if (_isWaiting)
        {
            _waveManager.StartSpawn();
            _timerButton.gameObject.SetActive(false);
            _timerText.gameObject.SetActive(false);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_currentTimer / 60f);
        int seconds = Mathf.FloorToInt(_currentTimer % 60f);
        _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void OnTimerButtonClicked()
    {
        _isWaiting = false;
        
        int coins = CalculateCoins();
        // Выполните действие с монетами, например, добавьте их к игроку
        
        Debug.Log("Coins earned: " + coins);
        
        Spawn();
        _timerButton.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
    }

    private int CalculateCoins()
    {
        float percentage = 1f - (_currentTimer / _totalTime);
        int coins = Mathf.RoundToInt(percentage * _maxCoins);
        return coins;
    }

    private void Spawn()
    {
        _buttonStartSpawning.gameObject.SetActive(false);
        _waveManager.StartSpawn();
    }
    
}
