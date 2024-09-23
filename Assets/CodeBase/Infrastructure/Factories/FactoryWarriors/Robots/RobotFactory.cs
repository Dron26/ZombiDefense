using Characters.Robots;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.GeneralFactory;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Robots
{
    public class RobotFactory : MonoCache, IServiceFactory
    {
        private AudioManager _audioManager;
        public UnityAction<Turret> CreatedRobot;
private SaveLoadService _saveLoadService;
        public void Create(GameObject prefab, Transform transform )
        {
            GameObject newRobot = Instantiate(prefab, transform);
            Turret turretComponent = newRobot.GetComponent<Turret>();
            Transform newTurret = turretComponent.transform;
            newTurret.localPosition = Vector3.zero;
            newTurret.tag="PlayerUnit";
            turretComponent.OnInitialize += OnInitialized;
            float randomAngle = Random.Range(0f, 360f);
            newTurret.rotation = Quaternion.Euler(0f, randomAngle, 0f);
            TurretWeaponController turretWeaponController  = newTurret.GetComponent<TurretWeaponController>();
            turretWeaponController.Initialize();
           // turretComponent.Initialize(_audioManager, _saveLoadService);
            
        }

        private void OnInitialized( Turret turretComponent)
        {
            CreatedRobot?.Invoke(turretComponent);
        }
        
        public void Initialize( AudioManager audioManager,SaveLoadService saveLoadService)
        {
            _audioManager=audioManager;
            _saveLoadService=saveLoadService;
        }
    }
}