using System;
using System.Collections.Generic;
using Data;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Tutorial;
using Service;
using Service.Audio;
using Service.SaveLoad;
using UI.HUD.StorePanel;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.Logic.Inits
{
    public class SceneInitializer : MonoCache
    {
       
        [SerializeField] private Store store;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private List<Image> _images;
        
        private SaveLoadService _saveLoadService;
        private PlayerCharacterInitializer _playerCharacterInitializer;
        private EnemyCharacterInitializer _enemyCharacterInitializer;
         private GameObject _location;
         private Camera _cameraPhysical;
         private Camera _cameraUI;

        private GameBootstrapper _gameBootstrapper;
        private LoadingCurtain _loadingCurtain;
        private WaveManager _waveManager;
        private GameStateMachine _stateMachine;
        private int ordered = 1;
        public UnityAction SetInfoCompleted;
        
        public List<Humanoid> availableCharacters = new ();
        private bool _isTutorialLevel;

        public Action OnLoaded;
        public void Initialize(GameStateMachine stateMachine)
        {
            
            Debug.Log("SceneInitializer().Initialize");
            _stateMachine = stateMachine;
            _gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSAaveLoad();

            LoadLevelPrefab();
            
            LoadCharacters();
            Debug.Log("Finish LoadCharacters();");
            _saveLoadService.SetAvailableCharacters(availableCharacters);
            
            Debug.Log("Finish SetAvailableCharacters();");
            _saveLoadService.SetCameras(_cameraPhysical, _cameraUI);
            Debug.Log("Finish SetCameras();");

            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();

            _loadingCurtain.OnClicked = OnClikedCurtain;

            _playerCharacterInitializer.CreatedHumanoid += SetInfo;
            Debug.Log("Finish _playerCharacterInitializer();");
             _audioManager.Initialize(_saveLoadService);
            
             _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoadService);
             Debug.Log("Finish _playerCharacterInitializer();");

             _enemyCharacterInitializer.Initialize(_saveLoadService, this);
             Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager = _enemyCharacterInitializer.GetWaveManager();
            Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager.OnReadySpawning += OnReadySpawning;
            Debug.Log("Finish _playerCharacterInitializer();");

            _playerCharacterInitializer.AreOverHumanoids += _enemyCharacterInitializer.StopSpawning;
            Debug.Log("Finish _playerCharacterInitializer();");

            _movePointController.Initialize(this, _saveLoadService);
            Debug.Log("Finish _movePointController();");
  
            store.Initialize(this, _saveLoadService);
         //   _timerDisplay.Initialize(_playerCharacterInitializer);
         Debug.Log("Finish store();");
            
         _resursesCanvas.Initialize(_saveLoadService);
//_loadingCurtain.OnLoaded();
            Debug.Log("Finish _resursesCanvas();");

            _timeManager.Initialize();
            Debug.Log("Finish _timeManager();");

            _menuPanel.Initialize(_saveLoadService,_stateMachine);
            Debug.Log("finish _menuPanel().Initialize");

            StartCoroutine(_waveManager.SpawnWaves());

            StartTimer();
        }

        private void StartTimer()
        {
            if (_isTutorialLevel) return;
            _timerDisplay.StartTimer(_saveLoadService);
            _timerDisplay.OnClickStartSpawn+=_enemyCharacterInitializer.StartSpawning;
        }
        
        private void OnReadySpawning()
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

        private void LoadLevelPrefab()
        {
            string path = _saveLoadService.GetSelectedLocation().Path;
            GameObject tempLocation = Resources.Load<GameObject>(path);
            GameObject location =Instantiate(tempLocation);
            
            _playerCharacterInitializer=location.GetComponentInChildren<PlayerCharacterInitializer>();
            _enemyCharacterInitializer=location.GetComponentInChildren<EnemyCharacterInitializer>();
            _cameraPhysical=location.GetComponentInChildren<Camera>();
            _cameraUI=location.GetComponentInChildren<Camera>();
            _location=location;
            tempLocation=null;
            
            if (_saveLoadService.GetSelectedLocation().IsTutorial)
            {
                _isTutorialLevel=true;
                TutorialLevel tutorialLevel = location.GetComponent<TutorialLevel>();
                tutorialLevel.SetImages(GetImages());
                OnLoaded+=()=> tutorialLevel.Initialize(this);
                tutorialLevel.OnEndTutorial+=SwicthScene;
                //   location.GetComponent<TutorialLevel>().Initialize();
            }
            else
            {
                _isTutorialLevel = false;
            }
        }

         protected override void OnDisabled()
        {
            _playerCharacterInitializer.CreatedHumanoid -= SetInfo;
            _playerCharacterInitializer.AreOverHumanoids -= _enemyCharacterInitializer.StopSpawning;
            _timerDisplay.OnClickStartSpawn-=_enemyCharacterInitializer.StartSpawning;
            _waveManager.OnReadySpawning -= OnReadySpawning;
        }

         public List<Image> GetImages()
         {
             return _images;
         }
         
         private void SwicthScene()
         {
             _saveLoadService.Save();
             _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Menu); 
             Destroy(gameObject);
         }    }
}