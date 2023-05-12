using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
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
        private List<Button> _workPointButtons = new ();
        private static readonly List<Humanoid> _activeHumanoids = new();
        private static readonly List<Humanoid> _inactiveHumanoids = new();
        public UnityAction AreOverHumanoids;
        public UnityAction<WorkPoint> OnClickWorkpoint;
        public void Initialize(AudioSource audioSource)
        {
            _humanoidFactory.Initialize(audioSource);
            _humanoidFactory.CreatedHumanoid += FillCharacterGroup;
            FillWorkPoints();
        }

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointsGroup.WorkPoints)
            {
                _workPoints.Add(workPoint);
                workPoint.OnClick=OnClickWorkpoint;
            }
        } 
        
        
        private void FillCharacterGroup(Humanoid humanoid)
        {
            _activeHumanoids.Add(humanoid);
            DieState dieState = humanoid.GetComponent<DieState>();
            dieState.OnDeath += OnDeath;
        }

        private void CreateHumanoid(GameObject humanoid )
        {
                _humanoidFactory.Create(humanoid);
        }
        
        public void SetCreatHumanoid(GameObject humanoid)
        {
            if (humanoid != null&&humanoid.GetComponent<Humanoid>())
            {
                _humanoidFactory.Create(humanoid);
            }
            else
            {
                print("SetCreatHumanoid error");
            }
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

        public List<WorkPoint> GetWorkPointGroup() => new List<WorkPoint>(_workPoints);

        public HumanoidFactory GetFactory() => _humanoidFactory;
        //public Humanoid GetAvailableHumanoid() => 
        
        public List<Button> GetWorkPointButtons=>new List<Button>(_workPointButtons);
    }
}