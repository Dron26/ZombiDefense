using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Characters.Robots;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Humanoids;
using Infrastructure.Factories.FactoryWarriors.Robots;
using Infrastructure.Location;
using Infrastructure.Points;
using Service.Audio;
using Service.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.Inits
{
    public class PlayerCharacterInitializer : MonoCache
    {
        [SerializeField] private WorkPointGroup _workPointsGroup;
        [SerializeField] private HumanoidFactory _humanoidFactory;
        [SerializeField] private RobotFactory _robotFactory;
        private List<WorkPoint> _workPoints = new();
        private static readonly List<Character> _activeCharacter = new();
        private static readonly List<Character> _inactiveHumanoids = new();
        
        public UnityAction CreatedCharacter;
        private Humanoid _selectedHumanoid;
        private Store _store;
        private CharacterStore _characterStore;
        private CharacterStore _vipCharacterStore;
        private MovePointController _movePointController;
        public int CoutnCreated => _coutnCreated;
        private int _coutnCreated;
        public int CoutnOrdered => _countOrdered;
        private int _countOrdered;
        private SaveLoadService _saveLoadService;

        public Action LastHumanoidDie;

        public void Initialize(AudioManager audioManager, SceneInitializer sceneInitializer, SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _humanoidFactory.CreatedHumanoid += OnCreatedCharacted;
            _robotFactory.CreatedRobot += OnCreatedCharacted;
            _humanoidFactory.Initialize(audioManager);
            _robotFactory.Initialize(audioManager,_saveLoadService);
            _workPointsGroup.Initialize(_saveLoadService);
            FillWorkPoints();
            _store = sceneInitializer.Window.GetStoreOnPlay();
            
            _characterStore = _store.GetCharacterStore();
            _characterStore.OnCharacterBought += CreateCharacter;
            
            _vipCharacterStore = _store.GetVipCharacterStore();
            _vipCharacterStore.OnCharacterBought += CreateCharacter;
            
            _movePointController = sceneInitializer.GetMovePointController();
        }
        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointsGroup.GetWorkPoints())
            {
                _workPoints.Add(workPoint);
            }
        }

        private void OnCreatedCharacted(Character character)
        {
            _coutnCreated++;
            _activeCharacter.Add(character);
            
            if (character.TryGetComponent(out Humanoid humanoid))
            {
                DieState dieState = humanoid.GetComponent<DieState>();
                dieState.OnDeath += OnDeath;
            }
           
            CreatedCharacter?.Invoke();
            _movePointController.SelectedPoint.SetCharacter(character);
            _movePointController.SetCurrentPoint(_movePointController.SelectedPoint);
            _movePointController.SelectedPoint.OnPointerClick(null);
            _movePointController.SelectedPoint.OnPointerClick(null);
            SetLocalParametrs();
        }

        private  void CreateHumanoid(Humanoid humanoid, Transform transform)
        {
             _humanoidFactory.Create(humanoid.gameObject, transform);
        }
        
        private  void CreateTurret(Turret turret, Transform transform)
        {
            _robotFactory.Create(turret.gameObject, transform);
        }

        public void CreateCharacter(Character character )
        {
            Transform transform = _movePointController.SelectedPoint.transform;
            
            if (character.TryGetComponent( out Humanoid humanoid))
            {
                if (humanoid != null && humanoid.GetComponent<Humanoid>())
                {
                    _countOrdered++;
                    CreateHumanoid(humanoid, transform);
                }
                else
                {
                    print("SetCreatHumanoid error");
                }

            
                _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
            }
            else if(character.TryGetComponent( out Turret turret))
            {
                if (turret != null && turret.GetComponent<Turret>())
                {
                    _countOrdered++;
                    CreateTurret(turret, transform);
                }
                else
                {
                    print("SetCreatTurret error");
                }

            
                _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
            }
        }

        public List<Character> GetAllCharacter() => _activeCharacter;

        private void CheckRemainingHumanoids()
        {
            if (_activeCharacter.Count == 0)
            {
                LastHumanoidDie?.Invoke();
            }
        }

        private void OnDeath(Character character)
        {
            _inactiveHumanoids.Add(character);
            _activeCharacter.Remove(character);
            SetLocalParametrs();
            CheckRemainingHumanoids();
        }

        protected override void OnDisable()
        {
            foreach (Character character in _activeCharacter)
            {
                if (character.TryGetComponent(out Humanoid humanoid))
                {
                    DieState dieState = humanoid.GetComponent<DieState>();
                    dieState.OnDeath -= OnDeath;
                }
            }
        }

        public HumanoidFactory GetFactory() =>
            _humanoidFactory;

        public WorkPointGroup GetWorkPointGroup() =>
            _workPointsGroup;

        public Humanoid GetSelectedCharacter()
        {
            return _selectedHumanoid;
        }

        private void SetLocalParametrs()
        {
            _saveLoadService.SetActiveCharacters(_activeCharacter);
            _saveLoadService.SetInactiveHumanoids(_inactiveHumanoids);
        }
    }
}