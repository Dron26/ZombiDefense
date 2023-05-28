using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WaveManagment;
using Service.SaveLoadService;
using UI.SceneBattle.Store;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class SceneInitializer:MonoCache
    {
        private SaveLoad _saveLoad;
        
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private EnemyCharacterInitializer _enemyCharacterInitializer;
        [SerializeField] private StoreOnPlay _storeOnPlay;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        [SerializeField] private TimerDisplay _timerDisplay;
        int ordered = 1;
        public UnityAction SetInfoCompleted;
        
        private void Start()
        {
            _saveLoad = GetComponent<SaveLoad>();
            _saveLoad.SetAvailableCharacters(_avaibelCharacters);
            _playerCharacterInitializer.CreatedHumanoid+= SetInfo;
          //  _audioManager.Initialize(_saveLoad);
            _playerCharacterInitializer.Initialize(_audioManager,this,_saveLoad);
            _enemyCharacterInitializer.Initialize(_saveLoad,this);
            _playerCharacterInitializer.AreOverHumanoids+=_enemyCharacterInitializer.StopSpawning;
           _movePointController.Initialize(this,_saveLoad);
           _storeOnPlay.Initialize(this,_saveLoad);
           _timerDisplay.Initialize(_playerCharacterInitializer);
        }
        
        public PlayerCharacterInitializer GetPlayerCharacterInitializer() => _playerCharacterInitializer;

       
        private void  SetInfo()
        {
            int countCreated = _playerCharacterInitializer.CoutnCreated;

            if (ordered==countCreated)
            {
                SetInfoCompleted?.Invoke();
            }
        }
       
        public StoreOnPlay GetStoreOnPlay() => _storeOnPlay;
        public MovePointController GetMovePointController() => _movePointController;
        public SaveLoad GetSaveLoad() => _saveLoad;
        public AudioManager GetAudioController() => _audioManager;
        
    }
}