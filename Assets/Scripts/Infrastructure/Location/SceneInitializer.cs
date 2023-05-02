using System.Collections.Generic;
using System.Numerics;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WaveManagment;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace Infrastructure.Location
{
    public class SceneInitializer:MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private WorkPointGroup WorkPoint;
        [SerializeField] List<GameObject> humanoids ;
        [SerializeField] private HumanoidFactory _humanoidFactory;
        public UnityAction SetInfoCompleted;
        private void Start()
        {
            _waveManager.SpawningCompleted += SetInfo;
            
            CharacterInitialize();
            _waveManager.Initialize();
            _humanoidFactory.SetEnemyData (_waveManager.GetEnemyGroup());
            _waveManager.SetHumanoidData(_humanoidFactory.GetAllHumanoids());
            SetInfoCompleted?.Invoke();
        }
        
        public void CharacterInitialize()
        {
            List<Vector3> positions = new List<Vector3>();


            foreach (GameObject position in WorkPoint.WorkPoints)
            {
                positions.Add(position.transform.position);
            }
            
            _humanoidFactory.Create(positions[0],humanoids[0]);
            
            // for (int i = 0; i < humanoids.Count; i++)
            // {
            //     _humanoidFactory.Create(positions[i],humanoids[i]);
            // }
        }

        public HumanoidFactory GetHumanoidFactory() => _humanoidFactory;
        public EnemyFactory GetEnemyFactory() => _waveManager.GetWEnemyFactory();
        private void SetInfo()
        {
            
        }
    }
}