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
        
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private StoreOnPlay _storeOnPlay;
        [SerializeField] private AudioController _audioController;
        [SerializeField] private List<Humanoid> _avaibelCharacters;
        [SerializeField] private MovePointController _movePointController;
        int ordered = 1;
        [SerializeField] private Humanoid humanoid1;
        //[SerializeField] private GameObject humanoid2;
      //  [SerializeField] private GameObject humanoid3;
        public UnityAction SetInfoCompleted;
        
        private void Start()
        {
            _saveLoad = GetComponent<SaveLoad>();
            _saveLoad.SetAvailableCharacters(_avaibelCharacters);
            _playerCharacterInitializer.AreOverHumanoids+=StopSpawning;
            _playerCharacterInitializer.CreatedHumanoid+= SetInfo;
            _audioController.Initialize(_saveLoad);
            _playerCharacterInitializer.Initialize(_audioController,this,_saveLoad);

            //  _playerCharacterInitializer.SetCreatHumanoid(humanoid1);
         //   _playerCharacterInitializer.SetCreatHumanoid(humanoid2);
          //  _playerCharacterInitializer.SetCreatHumanoid(humanoid3);
            
            
           _waveManager.Initialize();
           _movePointController.Initialize(this,_saveLoad);
           _storeOnPlay.Initialize(this,_saveLoad);
        }
        
        public WaveSpawner GetWaveSpawner() => _waveManager.GetWaveSpawner();
        public PlayerCharacterInitializer GetPlayerCharacterInitializer() => _playerCharacterInitializer;

        private void StopSpawning()
        {
            _waveManager.StopSpawn();
        }
        private void  SetInfo()
        {
            int countCreated = _playerCharacterInitializer.CoutnCreated;

            if (ordered==countCreated)
            {
                SetInfoCompleted?.Invoke();
            }
        }

        // public List<Humanoid> GetAvaibelCharacters()
        // {
        //     return _avaibelCharacters;
        // }
       
        public StoreOnPlay GetStoreOnPlay() => _storeOnPlay;
        public MovePointController GetMovePointController() => _movePointController;
        public SaveLoad GetSaveLoad() => _saveLoad;
    }
}