using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Characters.Robots;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Robots;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Infrastructure.Points;
using Interface;
using Services;
using Services.Audio;
using Services.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.Inits
{
    public class PlayerCharacterInitializer : MonoCache
    {
        [SerializeField] private WorkPointGroup _workPointsGroup;
        [SerializeField] private RobotFactory _robotFactory;
        private List<WorkPoint> _workPoints = new();
        private SceneObjectManager _sceneObjectManager;
        public UnityAction CreatedCharacter;
        private Humanoid _selectedHumanoid;
        private Store _store;
        private CharacterStore _characterStore;
        private CharacterStore _vipCharacterStore;
        private MovePointController _movePointController;
        public int CoutnCreated => _coutnCreated;
        private int _coutnCreated;
        private int _countOrdered;
        private ICharacterHandler _characterHandler;
        private ISearchService _searchService;
        private IGameEventBroadcaster _eventBroadcaster;
        public Action LastHumanoidDie;
        private int _initialPrecentUp=50;

        public void Initialize(AudioManager audioManager, SceneInitializer sceneInitializer ,SceneObjectManager sceneObjectManager)
        {
            _sceneObjectManager=sceneObjectManager;
            _sceneObjectManager.CreatedHumanoid +=  OnCreatedCharacted;
            _robotFactory.CreatedRobot += OnCreatedCharacted;
            _robotFactory.Initialize(audioManager);
            _workPointsGroup.Initialize();
            FillWorkPoints();
            _store = sceneInitializer.Window.GetStoreOnPlay();
            _movePointController = sceneInitializer.GetMovePointController();
            _characterHandler= AllServices.Container.Single<ICharacterHandler>();
            _searchService= AllServices.Container.Single<ISearchService>();
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
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
            _movePointController.SelectedPoint.SetCharacter(character);
            _movePointController.SetCurrentPoint(_movePointController.SelectedPoint);
            
            SetCreatedCharacter(character);
            _searchService.AddEntity(character);
            
            if (character.TryGetComponent(out Humanoid humanoid))
            {
                humanoid.OnEntityDeath += OnDeath;
                _eventBroadcaster.InvokeOnSetActiveHumanoid();
            }
            if (character.TryGetComponent(out Turret turret))
            {
                turret.SetSaveLoadService();
            }
            
            _eventBroadcaster.InvokeOnSetActiveCharacter(character);
            CreatedCharacter?.Invoke();
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
                _workPointsGroup.OnSelected(_movePointController.SelectedPoint);
            }
        }
        
        private void CheckRemainingHumanoids()
        {
            if (_characterHandler.GetActiveCharacters().Count == 0)
            {
                LastHumanoidDie?.Invoke();
            }
        }

        private void OnDeath(Entity entity)
        {
            _eventBroadcaster.InvokeOnCharacterDie(entity.GetComponent<Character>());
            CheckRemainingHumanoids();
        }

        public WorkPointGroup GetWorkPointGroup() =>
            _workPointsGroup;

        public Humanoid GetSelectedCharacter()
        {
            return _selectedHumanoid;
        }

        public void ClearData()
        {
            _countOrdered = 0;
            _coutnCreated = 0;
        }
        
        private void AddListener()
        {
            _eventBroadcaster.OnCharacterLevelUp+=OnCharacterLevelUp;
        }

        private void OnCharacterLevelUp()
        {
            _selectedHumanoid.HealthLevelUp(_initialPrecentUp);
            _selectedHumanoid.GetComponent<HumanoidWeaponController>().DamageLevelUp(_initialPrecentUp);
        }

        protected override void OnDisable()
        {
            RemoveListener();
        }

        private void RemoveListener()
        {
            _eventBroadcaster.OnCharacterLevelUp-=OnCharacterLevelUp;
        }
    }
}