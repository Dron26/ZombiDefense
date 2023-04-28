using Humanoids.People;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryPeople
{
    public class KingFactory : PeopleFactory
    {
        [SerializeField] private King _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.King));
    }
}