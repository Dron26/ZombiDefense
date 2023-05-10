using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.WeaponManagment;
using Service.GeneralFactory;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids
{ 
    enum Level
    {
        Soldier = 1,
        Archer = 2,
        Knight = 3,
        King = 4,
        
        CyberSoldier = 5,
        CyberArcher = 6,
        CyberKnight = 7,
        CyberKing = 8,
        
        CrazyTractor = 9,
        CyberZombie = 10,
        GunGrandmother = 11,
        Virus = 12
    }
    
    public class HumanoidFactory : MonoCache, IServiceFactory
    {
        [SerializeField] private List<HumanoidData> humanoidsData;
        private static readonly List<Humanoid> _inactiveHumanoids = new();
        [SerializeField] private  List<WorkPointGroup> _workPoints;
        private AudioSource _audioSource;
        
        
        public Humanoid Create(WorkPoint workPoint, GameObject humanoid)
        {
            GameObject newHumanoid = Instantiate(humanoid, workPoint.transform.position, Quaternion.identity);
            newHumanoid.transform.parent=workPoint.transform;
            Humanoid humanoidComponent = newHumanoid.GetComponent<Humanoid>();
            humanoidComponent.Load += SetWeapons;
            humanoidComponent.LoadPrefab();
            
            return humanoidComponent;
        }

        private void SetWeapons(Humanoid humanoid )
        {
            WeaponController weaponController = humanoid.GetComponent<WeaponController>();
            weaponController.Initialize();
            FXController fxController = humanoid.GetComponent<FXController>();
            fxController.SetAudioSource(_audioSource);
            fxController.Initialize(weaponController);
        }

        public HumanoidData GetRandomHumanoidData(int level)
        {
            var humanoidDatas = new List<HumanoidData>();
            
            foreach (var enemy in humanoidsData)
            {
                if (enemy.Level == level)
                {
                    humanoidDatas.Add(enemy);
                }
            }
    
            if (humanoidDatas.Count > 0)
            {
                var index = Random.Range(0, humanoidDatas.Count);
                return humanoidDatas[index];
            }
            else
            {
                Debug.LogError($"No enemies found with level {level}.");
                return null;
            }
        }

        //при убийстве персонажа игрока, нужно обновлять список доступных персонажей. как вариант отдавать не через фабрику список  ну или на фабрике подписываться и перекидывать в лист дохлых а энеми будут искать новых таргетов.

        public void Initialize(AudioSource audioSource)
        {
            _audioSource=audioSource;
        }
    }
}