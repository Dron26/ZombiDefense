using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.StateMachine;
using Service;
using Service.Audio;
using Service.SaveLoad;
using UI.HUD.StorePanel;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.Inits
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

        public void Initialize(GameStateMachine stateMachine)
        {
            Debug.Log("SceneInitializer().Initialize");
            _stateMachine = stateMachine;
            _gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSAaveLoad();

            LoadCharacters();
            Debug.Log("Finish LoadCharacters();");
            _saveLoadService.SetAvailableCharacters(availableCharacters);
            
            Debug.Log("Finish SetAvailableCharacters();");
            _saveLoadService.SetCameras(_cameraPhysical, _cameraUI);
            Debug.Log("Finish SetCameras();");

            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();

            _loadingCurtain.OnClicked = OnClikedCurtain;

            _moneyData=_saveLoadService.MoneyData;

            _playerCharacterInitializer.CreatedHumanoid += SetInfo;
            Debug.Log("Finish _playerCharacterInitializer();");
             _audioManager.Initialize(_saveLoadService);
            
             _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoadService);
             Debug.Log("Finish _playerCharacterInitializer();");

             _enemyCharacterInitializer.Initialize(_saveLoadService, this);
             Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager = _enemyCharacterInitializer.GetWaveManager();
            Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager.OnReadySpawning = OnReadySpawning;
            Debug.Log("Finish _playerCharacterInitializer();");

            _playerCharacterInitializer.AreOverHumanoids += _enemyCharacterInitializer.StopSpawning;
            Debug.Log("Finish _playerCharacterInitializer();");

            _movePointController.Initialize(this, _saveLoadService);
            Debug.Log("Finish _movePointController();");
  
            store.Initialize(this, _saveLoadService,_moneyData);
         //   _timerDisplay.Initialize(_playerCharacterInitializer);
         Debug.Log("Finish store();");
            
         _resursesCanvas.Initialize(_saveLoadService);
//_loadingCurtain.OnLoaded();
            Debug.Log("Finish _resursesCanvas();");

            _timeManager.Initialize();
            Debug.Log("Finish _timeManager();");

            _menuPanel.Initialize(_saveLoadService,_stateMachine);
            Debug.Log("finish _menuPanel().Initialize");
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
            Debug.Log("Cliked Curve");
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