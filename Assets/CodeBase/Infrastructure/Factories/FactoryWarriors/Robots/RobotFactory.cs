using Characters.Humanoids.AbstractLevel;
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
        private AudioManager _audioManager;
        public UnityAction<Robot> CreatedRobot;

        public void Create(GameObject prefab, Transform transform )
        {
            GameObject newRobot = Instantiate(prefab, transform);
            Robot robotComponent = newRobot.GetComponent<Robot>();
            WeaponController weaponController  = newRobot.GetComponent<WeaponController>();
            
            robotComponent.transform.localPosition = Vector3.zero;
            robotComponent.OnInitialize += OnInitialized;
            float randomAngle = Random.Range(0f, 360f);
            newRobot.transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
            weaponController.Initialize();
            robotComponent.Initialize(_audioManager);

        }

        private void OnInitialized( Robot robotComponent)
        {
            CreatedRobot?.Invoke(robotComponent);
        }
        
        public void Initialize( AudioManager audioManager)
        {
            _audioManager=audioManager;
        }
        
    }
}