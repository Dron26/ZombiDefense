using System.Collections.Generic;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.States;
using Infrastructure.WaveManagment;
using Service;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.Events;
using Upgrades;

namespace Infrastructure.Location
{
    public class SceneInitializer : MonoCache
    {
        private SaveLoadService _saveLoadService;
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private EnemyCharacterInitializer _enemyCharacterInitializer;
        [SerializeField] private Store store;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private MenuPanel _menuPanel;
        private MoneyData _moneyData;
        
        [SerializeField] private Camera _cameraPhysical;
        [SerializeField] private Camera _cameraUI;

        private GameBootstrapper _gameBootstrapper;
        private LoadingCurtain _loadingCurtain;
        private WaveManager _waveManager;
        private GameStateMachine _stateMachine;
        private int ordered = 1;
        public UnityAction SetInfoCompleted;
        
        public string characterFolderPath = "Assets/NewArmy/Characters/Completed";
        public List<Humanoid> availableCharacters = new ();

        public async void Initialize(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSAaveLoad();
            LoadCharacters();
            _saveLoadService.SetAvailableCharacters(availableCharacters);
            _saveLoadService.SetCameras(_cameraPhysical, _cameraUI);
            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();
            _loadingCurtain.OnClicked = OnClikedCurtain;
            _moneyData=new MoneyData(_saveLoadService);
            _playerCharacterInitializer.CreatedHumanoid += SetInfo;
             _audioManager.Initialize(_saveLoadService);
            _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoadService);
            _enemyCharacterInitializer.Initialize(_saveLoadService, this);
            _waveManager = _enemyCharacterInitializer.GetWaveManager();
            _waveManager.OnReadySpawning = OnReadySpawning;
            _playerCharacterInitializer.AreOverHumanoids += _enemyCharacterInitializer.StopSpawning;
            _movePointController.Initialize(this, _saveLoadService);
            store.Initialize(this, _saveLoadService,_moneyData);
            _timerDisplay.Initialize(_playerCharacterInitializer);
            _resursesCanvas.Initialize(_saveLoadService);
//_loadingCurtain.OnLoaded();

            _timeManager.Initialize();
            _menuPanel.Initialize(_saveLoadService,_stateMachine);
        }

        private void OnReadySpawning()
        {
            _loadingCurtain.OnLoaded();
        }

// public void Start( )
// // {
// //
// //     _saveLoad = GetComponent<SaveLoad>();
// //     _saveLoad.SetAvailableCharacters(_avaibelCharacters);
// //     _saveLoad.SetLevel(_level.GetWaveDataInfo());
// //    // _loadingCurtain=_gameBootstrapper.GetLoadingCurtain();
// //    // _loadingCurtain.OnClicked = OnClikedCurtain;
// //     _playerCharacterInitializer.CreatedHumanoid+= SetInfo;
// //     _audioManager.InitializeAsync(_saveLoad);
// //     _playerCharacterInitializer.Initialize(_audioManager,this,_saveLoad);
// //     _enemyCharacterInitializer.Initialize(_saveLoad,this);
// //     _playerCharacterInitializer.AreOverHumanoids+=_enemyCharacterInitializer.StopSpawning;
// //     _movePointController.Initialize(this,_saveLoad);
// //     _storeOnPlay.Initialize(this,_saveLoad);
// //     _timerDisplay.Initialize(_playerCharacterInitializer);
// // }

        private void OnClikedCurtain()
        {
            print("Cliked Curve");
        }

        public PlayerCharacterInitializer GetPlayerCharacterInitializer() => _playerCharacterInitializer;

        private void SetInfo()
        {
            int countCreated = _playerCharacterInitializer.CoutnCreated;
            if (ordered == countCreated)
            {
                SetInfoCompleted?.Invoke();
            }
        }

        public Store GetStoreOnPlay() => store;
        public MovePointController GetMovePointController() => _movePointController;
        public SaveLoadService GetSaveLoad() => _saveLoadService;
        public AudioManager GetAudioController() => _audioManager;
        
        
            
           
        
        
        private void LoadCharacters()
        {
            GameObject[] prefabs = Resources.LoadAll<GameObject>("CompletedCharacters");
            
            foreach (GameObject characterPrefab in prefabs)
            {
                Humanoid character = characterPrefab.GetComponent<Humanoid>();
                availableCharacters.Add(character);
            }
        }
        
    }
}