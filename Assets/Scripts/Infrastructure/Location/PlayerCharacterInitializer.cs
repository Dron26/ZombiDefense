using System.Collections.Generic;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Humanoids;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class PlayerCharacterInitializer : MonoCache
    {
        [SerializeField] private WorkPointGroup _workPointsGroup;
        [SerializeField] private HumanoidFactory _humanoidFactory;
        private List<WorkPoint> _workPoints = new();
        private static readonly List<Humanoid> _activeHumanoids = new();
        private static readonly List<Humanoid> _inactiveHumanoids = new();
        public UnityAction AreOverHumanoids;
        public UnityAction CreatedHumanoid;
        private Humanoid _selectedHumanoid;
        private Store store;
        private MovePointController _movePointController;
        public int CoutnCreated => _coutnCreated;
        private int _coutnCreated;
        public int CoutnOrdered => _countOrdered;
        private int _countOrdered;
        private SaveLoad _saveLoad;

        public void Initialize(AudioManager audioManager, SceneInitializer sceneInitializer, SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _humanoidFactory.CreatedHumanoid += OnCreatedHumanoid;
            _humanoidFactory.Initialize(audioManager);
            _workPointsGroup.Initialize(_saveLoad);
            FillWorkPoints();
            store = sceneInitializer.GetStoreOnPlay();
            store.BuyCharacter += SetCreatHumanoid;
            _movePointController = sceneInitializer.GetMovePointController();
        }

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointsGroup.GetWorkPoints())
            {
                _workPoints.Add(workPoint);
            }
        }

        private void OnCreatedHumanoid(Humanoid humanoid)
        {
            _coutnCreated++;
            _activeHumanoids.Add(humanoid);
            DieState dieState = humanoid.GetComponent<DieState>();
            dieState.OnDeath += OnDeath;
            CreatedHumanoid?.Invoke();
            SetLocalParametrs();
        }

        private async void CreateHumanoid(Humanoid humanoid, Transform transform)
        {
            await _humanoidFactory.Create(humanoid.gameObject, transform);
        }

        public void SetCreatHumanoid(Humanoid humanoid)
        {
            Transform transform = _movePointController.SelectedPoint.transform;
            if (humanoid != null && humanoid.GetComponent<Humanoid>())
            {
                _countOrdered++;
                CreateHumanoid(humanoid, transform);
            }
            else
            {
                print("SetCreatHumanoid error");
            }

            _movePointController.SelectedPoint.CheckState();
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

        protected override void OnDisable()
        {
            foreach (Humanoid humanoid in _activeHumanoids)
            {
                DieState dieState = humanoid.GetComponent<DieState>();
                dieState.OnDeath -= OnDeath;
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
            _saveLoad.SetActiveHumanoids(_activeHumanoids);
            _saveLoad.SetInactiveHumanoids(_inactiveHumanoids);
        }
    }
}