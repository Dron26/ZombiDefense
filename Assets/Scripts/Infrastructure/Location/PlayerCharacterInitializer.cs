using System;
using System.Collections.Generic;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using JetBrains.Annotations;
using Service.SaveLoadService;
using UI.SceneBattle.Store;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.Location
{
    public class PlayerCharacterInitializer : MonoCache
    {
        [SerializeField] private WorkPointGroup _workPointsGroup;
        [SerializeField] private HumanoidFactory _humanoidFactory;
        private List<WorkPoint> _workPoints = new ();
        private static readonly List<Humanoid> _activeHumanoids = new();
        private static readonly List<Humanoid> _inactiveHumanoids = new();
        public UnityAction AreOverHumanoids;
        public UnityAction CreatedHumanoid;
        private Humanoid _selectedHumanoid;
        private StoreOnPlay _storeOnPlay;
        private MovePointController _movePointController;
        
        public int CoutnCreated => _coutnCreated;
        private int _coutnCreated;
        public int CoutnOrdered => _countOrdered;
        
        private int _countOrdered;
        private SaveLoad _saveLoad;

        public void Initialize(AudioController audioController, SceneInitializer sceneInitializer, SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _humanoidFactory.CreatedHumanoid += FillCharacterGroup;
            _humanoidFactory.Initialize(audioController);
            FillWorkPoints();
            _storeOnPlay=sceneInitializer.GetStoreOnPlay();
            _storeOnPlay.BuyCharacter+=SetCreatHumanoid;
            _movePointController = sceneInitializer.GetMovePointController();
        }
        

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointsGroup.GetWorkPoints())
            {
                _workPoints.Add(workPoint);
            }
        }

        private void FillCharacterGroup(Humanoid humanoid)
        {
            _coutnCreated++;
            _activeHumanoids.Add(humanoid);
            humanoid.OnHumanoidSelected += OnHumanoidSelected;
            DieState dieState = humanoid.GetComponent<DieState>();
            dieState.OnDeath += OnDeath;
            CreatedHumanoid?.Invoke();
            SetLocalParametrs();
        }

        private void OnHumanoidSelected(Humanoid humanoid)
        {
            _selectedHumanoid=humanoid;
            _saveLoad.SetSelectedHumanoid(humanoid);
        }

        private  void CreateHumanoid(Humanoid humanoid, Transform transform)
        { 
            
            _humanoidFactory.Create(humanoid.gameObject,  transform);
        }
        
        public void SetCreatHumanoid( Humanoid humanoid)
        {
            Transform transform = _movePointController.SelectedPoint.transform;
           
            if (humanoid != null&&humanoid.GetComponent<Humanoid>())
            {
                _countOrdered++;
                CreateHumanoid(humanoid,transform);
            }
            else
            {
                print("SetCreatHumanoid error");
            }
            
            _movePointController.SelectedPoint.SetBusy(true);

            _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
        }

        public List<Humanoid> GetAllHumanoids() => _activeHumanoids;

        private void CheckRemaningHumanoids()
        {
            if (_activeHumanoids.Count == 0)
            {
                AreOverHumanoids?.Invoke();
            }
        }
        
        private void OnDeath(Humanoid humanoid)
        {
            _inactiveHumanoids.Add(humanoid);
            _activeHumanoids.Remove(humanoid);
            SetLocalParametrs();
            CheckRemaningHumanoids();
        }
        
        private void OnDisable()
        {
            foreach (Humanoid humanoid in _activeHumanoids)
            {
                DieState dieState = humanoid.GetComponent<DieState>();
                dieState.OnDeath -= OnDeath;
            }
        }
        

        public HumanoidFactory GetFactory() => _humanoidFactory;
        //public Humanoid GetAvailableHumanoid() => 

        public WorkPointGroup GetWorkPointGroup() => _workPointsGroup;

        public Humanoid GetSelectedCharacter()
        {
           return _selectedHumanoid;
        }

        private void SetLocalParametrs()
        {
            _saveLoad.SetActiveHumanoids(_activeHumanoids);
            _saveLoad.SetInactiveHumanoids(_inactiveHumanoids);
        }
    }
}