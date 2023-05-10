using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class PlayerCharacterInitializer : MonoCache
    {
        [SerializeField] private List<GameObject> humanoids;
        [SerializeField] private WorkPointGroup _workPointsGroup;
        [SerializeField] private HumanoidFactory _humanoidFactory;
        private List<WorkPoint> _workPoints = new ();
        private static readonly List<Humanoid> _activeHumanoids = new();
        private static readonly List<Humanoid> _inactiveHumanoids = new();
        public UnityAction AreOverHumanoids;
        
        public void CharacterInitialize(AudioSource audioSource)
        {
            _humanoidFactory.Initialize(audioSource);
            FillWorkPoints();
            SetHumanoidToGroup(audioSource);
        }

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointsGroup.WorkPoints)
            {
                _workPoints.Add(workPoint);
            }
        }
        
        private void SetHumanoidToGroup(AudioSource audioSource)
        {
            for (int i = 0; i < humanoids.Count; i++)
            {
                int index = _activeHumanoids.Count;
                _activeHumanoids.Add(_humanoidFactory.Create(_workPoints[i], humanoids[i]));
                _activeHumanoids[index].StartPosition = _workPoints[i].transform.position;
                DieState dieState = _activeHumanoids[index].GetComponent<DieState>();
                dieState.OnDeath += OnDeath;
            }
        }

        public HumanoidFactory GetHumanoidFactory() => _humanoidFactory;
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
    }
}