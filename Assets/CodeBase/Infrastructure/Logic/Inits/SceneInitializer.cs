using System;
using System.Collections.Generic;
using CameraMain;
using Characters.Humanoids.AbstractLevel;
using Data;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.Points;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Tutorial;
using Service.Ads;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        
        private LoadingCurtain _loadingCurtain;
        public HudPanel Window=>_hudPanel;
        private SaveLoadService _saveLoadService;
        private PlayerCharacterInitializer _playerCharacterInitializer;
        private EnemyCharacterInitializer _enemyCharacterInitializer;
         private GameObject _location;
         

        private GameBootstrapper _gameBootstrapper;
        private WaveManager _waveManager;
        private GameStateMachine _stateMachine;
        private int ordered = 1;
        public Action SetInfoCompleted;
        public Action OnReadySpawning;
        
        public List<Character> availableCharacters = new ();

        public bool IsStartedTutorial => _isTutorialLevel;
        private bool _isTutorialLevel;
        private bool _isInfinity;

        public Action OnLoaded;
        public void Initialize(GameStateMachine stateMachine)
        {
            
            Debug.Log("SceneInitializer().Initialize");
            _stateMachine = stateMachine;
            _gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSaveLoad();
            _saveLoadService.SetEvenSystem(_eventSystem);
            LoadLevelPrefab();
            
            LoadCharacters();
            Debug.Log("Finish LoadCharacters();");
            _saveLoadService.SetAvailableCharacters(availableCharacters);
            
            Debug.Log("Finish SetAvailableCharacters();");
            _saveLoadService.SetCameras(_cameraPhysical, _cameraUI);
            Debug.Log("Finish SetCameras();");
            
            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();

            _loadingCurtain.OnClicked = OnClikedCurtain;

            _playerCharacterInitializer.CreatedCharacter += SetInfo;
            Debug.Log("Finish _playerCharacterInitializer();");
             _audioManager.Initialize(_saveLoadService);

             _hudPanel.Init(_saveLoadService,this );
             _hudPanel.OnClickStartSpawn+=_enemyCharacterInitializer.StartSpawning;
             _hudPanel.OnClickContinueStartSpawn+=OnClickContinueStartSpawn;
             _hudPanel.OnClickExitToMenu+=OnClickExitToMenu;

             
             _playerCharacterInitializer.Initialize(_audioManager, this, _saveLoadService);
             Debug.Log("Finish _playerCharacterInitializer();");

             InitializeEnemies();


             _playerCharacterInitializer.LastHumanoidDie += OnLastHumanoidDie;
            Debug.Log("Finish _playerCharacterInitializer();");

            _movePointController.Initialize(this, _saveLoadService);
            Debug.Log("Finish _movePointController();");
            
            
        }

        private void OnLastHumanoidDie()
        {
            _saveLoadService.OnLastHumanoidDie();
            _enemyCharacterInitializer.StopSpawning();
        }


        private void InitializeEnemies()
        {
            Debug.Log("+++InitializeEnemies++++");
            _enemyCharacterInitializer.Initialize(_saveLoadService, this);
            Debug.Log("Finish _playerCharacterInitializer();");
            _waveManager = _enemyCharacterInitializer.GetWaveManager();
            Debug.Log("Finish _playerCharacterInitializer();");

            _waveManager.OnReadySpawning += ReadyToSpawning;
            Debug.Log("Finish _playerCharacterInitializer();");
            
            StartCoroutine(_waveManager.SetWaveData());
        }
       
        
        private void ReadyToSpawning()
        {
            _loadingCurtain.OnLoaded();
            OnReadySpawning?.Invoke();
            
            if (_isInfinity)
            {
                _enemyCharacterInitializer.StartSpawning();
                _isInfinity = false;
            }
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

        private void LoadLevelPrefab()
        {
            string path = _saveLoadService.GetSelectedLocation().Path;
            GameObject tempLocation = Resources.Load<GameObject>(path);
            GameObject location =Instantiate(tempLocation);
            
            _playerCharacterInitializer=location.GetComponentInChildren<PlayerCharacterInitializer>();
            _enemyCharacterInitializer=location.GetComponentInChildren<EnemyCharacterInitializer>();
            CameraData cameraData = location.GetComponentInChildren<CameraData>();
            _cameraInputMovement.SetPosition(cameraData); 
            _location=location;
            tempLocation=null;
            
            if (_saveLoadService.GetSelectedLocation().IsTutorial)
            {
                _isTutorialLevel=true;
                TutorialLevel tutorialLevel = location.GetComponent<TutorialLevel>();
               // tutorialLevel.SetImages(GetImages());
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
            _playerCharacterInitializer.CreatedCharacter -= SetInfo;
            _playerCharacterInitializer.LastHumanoidDie -= _enemyCharacterInitializer.StopSpawning;
            _waveManager.OnReadySpawning -= ReadyToSpawning;
            _hudPanel.OnClickStartSpawn-=_enemyCharacterInitializer.StartSpawning;
        }

         public HudPanel GetHudPanel()
         {
             return _hudPanel;
         }
         
         private void SwicthScene()
         {
             _saveLoadService.Save();
             _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Menu); 
             Destroy(gameObject);
         }


         public void OnClickContinueStartSpawn()
         {
             _saveLoadService.ClearSpawnData();
            InitializeEnemies();
            StartCoroutine(_waveManager.SetWaveData());
            _isInfinity=true;
            
         }
         
         private void OnClickExitToMenu()
         {
             _saveLoadService.Save();
             _saveLoadService.GetGameBootstrapper().GetStateMachine().Enter<LoadLevelState,string>(ConstantsData.Menu); 
             Destroy(_location.gameObject);
             Destroy(transform.parent.gameObject);
         }

         public object GetStore()
         {
             return _hudPanel.GetStoreOnPlay();
         }
    }
}