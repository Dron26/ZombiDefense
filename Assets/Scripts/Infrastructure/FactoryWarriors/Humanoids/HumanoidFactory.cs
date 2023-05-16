using System.Threading.Tasks;
using Audio;
using Cysharp.Threading.Tasks;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.GeneralFactory;
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
        private AudioController _audioController;
        public UnityAction<Humanoid> CreatedHumanoid; 
        
        
        
        public async Task Create(GameObject prefab, Transform transform )
        {
            GameObject newHumanoid = Instantiate(prefab, transform);
            Humanoid humanoidComponent = newHumanoid.GetComponent<Humanoid>();
            humanoidComponent.SetAudioController(_audioController);
            humanoidComponent.OnDataLoad = Created;
            await UniTask.SwitchToMainThread();
            await humanoidComponent.LoadPrefab();
        }

        private void Created( Humanoid humanoidComponent)
        {
            CreatedHumanoid?.Invoke(humanoidComponent);
        }
        
        public void Initialize( AudioController audioController)
        {
            _audioController=audioController;
        }
    }
}