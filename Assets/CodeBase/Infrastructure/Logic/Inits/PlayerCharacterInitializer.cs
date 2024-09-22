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
        private static readonly List<Character> _activeCharacters = new();
        private static readonly List<Character> _inactiveCharacetrs = new();
        private SceneObjectManager _sceneObjectManager;
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
            _sceneObjectManager.CreatedHumanoid += OnCreatedCharacted;
            _robotFactory.CreatedRobot += OnCreatedCharacted;
            _robotFactory.Initialize(audioManager,_saveLoadService);
            _workPointsGroup.Initialize(_saveLoadService);
            FillWorkPoints();
            _store = sceneInitializer.Window.GetStoreOnPlay();
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
            _activeCharacters.Add(character);
            SetCreatedCharacter(character);
            
            if (character.TryGetComponent(out Humanoid humanoid))
            {
                DieState dieState = humanoid.GetComponent<DieState>();
                dieState.OnDeath += OnDeath;
            }
           
            CreatedCharacter?.Invoke();
            _movePointController.SelectedPoint.SetCharacter(character);
            _movePointController.SetCurrentPoint(_movePointController.SelectedPoint);
            
            SetLocalParametrs();
        }

        private  void CreateTurret(Turret turret, Transform transform)
        {
            _robotFactory.Create(turret.gameObject, transform);
        }

        public void SetCreatedCharacter(Character character )
        {
            Transform point = _movePointController.SelectedPoint.transform;
            Transform characterTransform = character.transform;
            characterTransform.parent=point;
            characterTransform.localPosition = Vector3.zero;
            
            
            
            if (character.TryGetComponent( out Humanoid humanoid))
            {
                _countOrdered++;
                _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
            }
            else if(character.TryGetComponent( out Turret turret))
            {
                _countOrdered++;
            //    CreateTurret(turret, transform);
                _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
            }
        }

        public List<Character> GetAllCharacter() => _activeCharacters;

        private void CheckRemainingHumanoids()
        {
            if (_activeCharacters.Count == 0)
            {
                LastHumanoidDie?.Invoke();
            }
        }

        private void OnDeath(Character character)
        {
            _inactiveCharacetrs.Add(character);
            _activeCharacters.Remove(character);
            SetLocalParametrs();
            CheckRemainingHumanoids();
        }

        protected override void OnDisable()
        {
            foreach (Character character in _activeCharacters)
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
            _saveLoadService.SetActiveCharacters(_activeCharacters);
            _saveLoadService.SetInactiveHumanoids(_inactiveCharacetrs);
        }

        public void ClearData()
        {
            ClearGroup(_activeCharacters);
            ClearGroup(_inactiveCharacetrs);
            _countOrdered = 0;
            _coutnCreated = 0;
        }

        private void ClearGroup(List<Character> characters)
        {
            foreach (var character in characters)
            {
                Destroy(character);
            }
            
            characters.Clear();
        }
    }
}