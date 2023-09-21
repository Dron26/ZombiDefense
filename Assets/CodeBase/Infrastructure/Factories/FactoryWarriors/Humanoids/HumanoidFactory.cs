using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.GeneralFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Humanoids
{
    public class HumanoidFactory : MonoCache, IServiceFactory
    {
        private AudioManager _audioManager;
        public UnityAction<Humanoid> CreatedHumanoid;

        public void Create(GameObject prefab, Transform transform )
        {
            GameObject newHumanoid = Instantiate(prefab, transform);
            Humanoid humanoidComponent = newHumanoid.GetComponent<Humanoid>();
            humanoidComponent.SetAudioController(_audioManager);
            humanoidComponent.transform.localPosition = Vector3.zero;
            humanoidComponent.OnDataLoad += Created;
            float randomAngle = Random.Range(0f, 360f);
            newHumanoid.transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
            humanoidComponent.Initialize();
        }

        private void Created( Humanoid humanoidComponent)
        {
            CreatedHumanoid?.Invoke(humanoidComponent);
        }
        
        public void Initialize( AudioManager audioManager)
        {
            _audioManager=audioManager;
        }
    }
}