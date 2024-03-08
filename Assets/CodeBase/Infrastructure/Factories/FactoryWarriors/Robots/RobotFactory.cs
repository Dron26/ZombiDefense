using Characters.Robots;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using Service.GeneralFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Robots
{
    public class RobotFactory : MonoCache, IServiceFactory
    {
        [SerializeField] private AudioManager _audioManager;
        public UnityAction<Turret> CreatedRobot;

        public void Create(GameObject prefab, Transform transform )
        {
            GameObject newRobot = Instantiate(prefab, transform);
            Turret turretComponent = newRobot.GetComponent<Turret>();
            
            turretComponent.transform.localPosition = Vector3.zero;
            turretComponent.OnInitialize += OnInitialized;
            float randomAngle = Random.Range(0f, 360f);
            newRobot.transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
            turretComponent.Initialize(_audioManager);

        }

        private void OnInitialized( Turret turretComponent)
        {
            CreatedRobot?.Invoke(turretComponent);
        }
        
        public void Initialize( AudioManager audioManager)
        {
            _audioManager=audioManager;
        }
        
    }
}