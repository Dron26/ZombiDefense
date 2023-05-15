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
        public bool IsHumanoidSelected => isHumanoidSelected;
        private bool isHumanoidSelected=false;
        
        public int CoutnCreated => _coutnCreated;
        private int _coutnCreated;
        public int CoutnOrdered => _countOrdered;
        
        private int _countOrdered;

        public void Initialize(AudioController audioController)
        {
            _humanoidFactory.Initialize(audioController);
            _humanoidFactory.CreatedHumanoid += FillCharacterGroup;
            FillWorkPoints();
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
        }

        private void OnHumanoidSelected(Humanoid humanoid)
        {
            _selectedHumanoid=humanoid;
            isHumanoidSelected = true;
        }

        private  void CreateHumanoid(GameObject humanoid)
        { 
            _humanoidFactory.Create(humanoid);
        }
        
        public void SetCreatHumanoid(GameObject humanoid)
        {
            if (humanoid != null&&humanoid.GetComponent<Humanoid>())
            {
                _countOrdered++;
                CreateHumanoid(humanoid);
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
        

        public HumanoidFactory GetFactory() => _humanoidFactory;
        //public Humanoid GetAvailableHumanoid() => 

        public WorkPointGroup GetWorkPointGroup() => _workPointsGroup;

        public Humanoid GetSelectedCharacter()
        {
           return _selectedHumanoid;
        }
    }
}