using System.Collections.Generic;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.States;
using Infrastructure.WaveManagment;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
using UI.Resurse;
using UnityEngine;
using UnityEngine.Events;
using Upgrades;

namespace Infrastructure.Location
{
    public class SceneInitializer : MonoCache
    {
        private SaveLoad _saveLoad;
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private EnemyCharacterInitializer _enemyCharacterInitializer;
        [SerializeField] private Store store;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        
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
            _saveLoad = _gameBootstrapper.GetSAaveLoad();
            LoadCharacters();
            _saveLoad.SetAvailableCharacters(availableCharacters);
            _saveLoad.SetCameras(_cameraPhysical, _cameraUI);
            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();
            _loadingCurtain.OnClicked = OnClikedCurtain;
            _playerCharacterInitializer.CreatedHumanoid += SetInfo;
            await _audioManager.InitializeAsync(_saveLoad);
            _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoad);
            _enemyCharacterInitializer.Initialize(_saveLoad, this);
            _waveManager = _enemyCharacterInitializer.GetWaveManager();
            _waveManager.OnReadySpawning = OnReadySpawning;
            _playerCharacterInitializer.AreOverHumanoids += _enemyCharacterInitializer.StopSpawning;
            _movePointController.Initialize(this, _saveLoad);
            store.Initialize(this, _saveLoad);
            _timerDisplay.Initialize(_playerCharacterInitializer);
            _resursesCanvas.Initialize(_saveLoad);
//_loadingCurtain.OnLoaded();
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
        public SaveLoad GetSaveLoad() => _saveLoad;
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