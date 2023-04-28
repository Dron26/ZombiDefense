using Humanoids.People;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryPeople
{
    public class KnightFactory : PeopleFactory
    {
        [SerializeField] private Knight _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.Knight));
    }
}