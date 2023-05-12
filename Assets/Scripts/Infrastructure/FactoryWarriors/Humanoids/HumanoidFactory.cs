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
using UnityEngine.Events;

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
        private AudioSource _audioSource;
        public UnityAction<Humanoid> CreatedHumanoid; 
        
        public void Create(GameObject humanoid)
        {
            GameObject newHumanoid = Instantiate(humanoid, transform);
            newHumanoid.gameObject.SetActive(false);
            Humanoid humanoidComponent = newHumanoid.GetComponent<Humanoid>();
            humanoidComponent.Load += SetComponent;
            humanoidComponent.LoadPrefab();
            
            if (humanoidComponent == null)
            {
                Debug.LogError($"PrefabCharacter {humanoidComponent.name} doesn't have a component of type Enemys.");
                Destroy(humanoidComponent);
            }
        }

        private void SetComponent(Humanoid humanoid )
        {
            AnimController animController = humanoid.GetComponent<AnimController>();
            animController.Initialize();
            WeaponController weaponController = humanoid.GetComponent<WeaponController>();
            weaponController.Initialize();
            FXController fxController = humanoid.GetComponent<FXController>();
            fxController.SetAudioSource(_audioSource);
            fxController.Initialize(weaponController);
            CreatedHumanoid?.Invoke(humanoid);
        }
       
            
        
        public void Initialize(AudioSource audioSource)
        {
            _audioSource=audioSource;
        }
    }
}