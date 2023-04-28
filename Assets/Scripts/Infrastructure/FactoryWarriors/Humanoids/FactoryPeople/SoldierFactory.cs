using Humanoids.People;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryPeople
{
    public class SoldierFactory : PeopleFactory
    {
        [SerializeField] private Soldier _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.Soldier));
    }
}