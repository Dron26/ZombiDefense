using Characters.Humanoids.AbstractLevel;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.GeneralFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Humanoids
{
    public class CharacterFactory : MonoCache, IServiceFactory
    {
        private AudioManager _audioManager;
        public UnityAction<Character> CreatedHumanoid;

        public GameObject Create(CharacterType type )
        {
            //string path =AssetPaths.CharactersPrefab + type;
            string path;
            if (type!=CharacterType.Turret)
            {
                path = AssetPaths.CharactersPrefab+"Customizable";
                
            }
            else
            {
                path = AssetPaths.CharactersPrefab+"Turret";
            }
           
            GameObject prefab = Instantiate(Resources.Load<GameObject>(path));
            prefab.gameObject.layer = LayerMask.NameToLayer("Character");
            return prefab;
        }
    }
}