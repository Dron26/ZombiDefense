using Humanoids.People;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryPeople
{
    public class ArcherFactory : PeopleFactory
    {
        [SerializeField] private Archer _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.Archer));
    }
}