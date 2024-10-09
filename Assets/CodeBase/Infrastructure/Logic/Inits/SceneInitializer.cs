using System;
using System.Collections.Generic;
using CameraMain;
using Characters.Humanoids.AbstractLevel;
using Common;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.Points;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Tutorial;
using Service.Ads;
using Service.Audio;
using Service.SaveLoad;
using UI.Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Infrastructure.Logic.Inits
{
    public class SceneInitializer : MonoCache
    {
       
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        [SerializeField] private List<Image> _images;
        [SerializeField] private MultiInputMovement _cameraInputMovement;
        [SerializeField] private HudPanel _hudPanel;
        [SerializeField] private Camera _cameraPhysical;
        [SerializeField] private Camera _cameraUI;
        [SerializeField] private EventSystem _eventSystem;
        private SceneObjectManager _sceneObjectManager;

         private LocationManager _locationManager;
        private LoadingCurtain _loadingCurtain;
        public HudPanel Window=>_hudPanel;
        private SaveLoadService _saveLoadService;
        private PlayerCharacterInitializer _playerCharacterInitializer;
         private GameObject _location;
         

        private GameBootstrapper _gameBootstrapper;
        private WaveManager _waveManager;
        private GameStateMachine _stateMachine;
        private int ordered = 1;
        public Action SetInfoCompleted;
        public Action OnReadySpawning;
        public Action OnClickContinue;
        private TimerDisplay _timerDisplay;
        public List<Character> availableCharacters = new ();

        public bool IsStartedTutorial => _isTutorialLevel;
        private bool _isTutorialLevel;
        

        public Action OnLoaded;
        public void Initialize(GameStateMachine stateMachine)
        {
            
            Debug.Log("SceneInitializer().Initialize");
            _stateMachine = stateMachine;
            _gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSaveLoad();
            _saveLoadService.SetEvenSystem(_eventSystem);
            _locationManager = _gameBootstrapper.GetLocationManager();
            LocationPrefab location= _locationManager.CreateLocation();
            SetInitializers(location);
            LoadCharacters();
            Debug.Log("Finish LoadCharacters();");
            _saveLoadService.SetAvailableCharacters(availableCharacters);
            
            Debug.Log("Finish SetAvailableCharacters();");
            _saveLoadService.SetCameras(_cameraPhysical, _cameraUI);
            Debug.Log("Finish SetCameras();");
            
            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();

            
            Debug.Log("Finish _playerCharacterInitializer();");
             _audioManager.Initialize(_saveLoadService);

             _timerDisplay=_hudPanel.GetTimerDisplay();
             
             _sceneObjectManager = GetComponent<SceneObjectManager>();
             _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoadService, _sceneObjectManager);
             Debug.Log("Finish _playerCharacterInitializer();");

             InitializeEnemies();
             AddListener();
             _hudPanel.Init(_saveLoadService,this,_waveManager,_locationManager );
             
            Debug.Log("Finish _playerCharacterInitializer();");

            _sceneObjectManager.Initialize( _hudPanel.GetStore(),_movePointController,_audioManager);
            Debug.Log("Finish _sceneObjectManager();");
            
            _movePointController.Initialize(this, _saveLoadService);
            Debug.Log("Finish _movePointController();");
            
        }

        private void SetInitializers(LocationPrefab location)
        {
            _playerCharacterInitializer = location.GetPlayer;
            _waveManager = location.GetEnemy;
            _cameraInputMovement.Initialize(location.CameraData); 
            LighInformer.SetLight(location.IsNight);
        }


        private void OnLastHumanoidDie()
        {
            _saveLoadService.OnLastHumanoidDie();
            _waveManager.StopSpawn();
        }
        
        private void InitializeEnemies()
        {
            Debug.Log("+++InitializeEnemies++++");
            _waveManager.Initialize(_saveLoadService, this);
            Debug.Log("Finish _playerCharacterInitializer();");
            Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager.OnReadySpawning += ReadyToSpawning;
            Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager.SetWaveData();
        }

        private void ReadyToSpawning()
        {
            _loadingCurtain.OnLoaded();
        }
        
        private void OnClikedCurtain()
        {
            OnLoaded?.Invoke();
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

        protected override void OnDisabled()
        {
            _playerCharacterInitializer.CreatedCharacter -= SetInfo;
            _playerCharacterInitializer.LastHumanoidDie -= _waveManager.StopSpawn;
            _waveManager.OnReadySpawning -= ReadyToSpawning;
            _timerDisplay.OnClickReady-=_waveManager.Spawn;        }

         public HudPanel GetHudPanel()
         {
             return _hudPanel;
         }
         
         private void SwicthScene()
         {
             _saveLoadService.Save();
             _stateMachine.Enter<LoadLevelState,string>(Constants.Menu); 
             Destroy(gameObject);
         }


         public void OnClickContinueStartSpawn()
         { 
             // _saveLoadService.ClearSpawnData();
             OnClickContinue?.Invoke();
           //  _waveManager.OnReadySpawning -= ReadyToSpawning;
            // _playerCharacterInitializer.ClearData();
            //InitializeEnemies();
            
       
         }
         
         private void OnClickExitToMenu()
         {
             _saveLoadService.Save();
             _saveLoadService.GetGameBootstrapper().GetStateMachine().Enter<LoadLevelState,string>(Constants.Menu); 
             _saveLoadService.ClearSpawnData();
             _playerCharacterInitializer.ClearData();
             //Destroy(_location.gameObject);
             Destroy(transform.parent.gameObject);
             
         }

         private void ResetLevel()
         {
             _saveLoadService.ClearSpawnData();
             _playerCharacterInitializer.ClearData();
            // Destroy(_location.gameObject);
            // Destroy(transform.parent.gameObject);
         }
         
         private void AddListener()
         {
             _timerDisplay.OnClickReady+=_waveManager.Spawn;
             _hudPanel.OnStartSpawn+=OnClickContinueStartSpawn;
             _hudPanel.OnClickExitToMenu+=OnClickExitToMenu;
             _hudPanel.OnResetLevel += ResetLevel;
             _loadingCurtain.OnClicked += OnClikedCurtain;
             _playerCharacterInitializer.CreatedCharacter += SetInfo;
             _playerCharacterInitializer.LastHumanoidDie += OnLastHumanoidDie;
         }

         private void RemoveListener()
         {
             _hudPanel.OnStartSpawn-=OnClickContinueStartSpawn;
             _hudPanel.OnClickExitToMenu-=OnClickExitToMenu;
             _hudPanel.OnResetLevel -= ResetLevel;
             _loadingCurtain.OnClicked -= OnClikedCurtain;
             _playerCharacterInitializer.CreatedCharacter -= SetInfo;
             _playerCharacterInitializer.LastHumanoidDie -= OnLastHumanoidDie;
         }

         private void OnDestroy()
         {
             RemoveListener();
         }
    }
}