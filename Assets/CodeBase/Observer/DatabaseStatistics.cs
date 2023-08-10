using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.Location;
using Infrastructure.WeaponManagment;
using Service.SaveLoadService;
using TMPro;
using UnityEngine;

namespace Observer
{
    public class DatabaseStatistics : MonoCache
    {
        [SerializeField] private SaveLoadService _saveLoadService;
        
        private const int GeneralCountMembers = 12;

        private readonly Dictionary<int, InfoMemberBattle> _membersBattles = new();
        
        public int TotalMoney { get; private set; }
        public int TotalPoints { get; private set; }
        private WeaponController _weaponController;
        public Dictionary<int, InfoMemberBattle> GetMembersBattle() =>
            _membersBattles;
        
        public void SetDataBase(PlayerCharacterInitializer playerCharacterInitializer)
        {
            Init();

            foreach (Humanoid humanoid in playerCharacterInitializer.GetAllHumanoids())
            {
                _weaponController = humanoid.GetComponent<WeaponController>();
                
                _membersBattles[humanoid.GetLevel()].DamageDone += humanoid.GetDamageDone();
                _membersBattles[humanoid.GetLevel()].DamageReceived += _weaponController.DamageReceived();
                
                TotalMoney += humanoid.GetDamageDone();
                TotalMoney += _weaponController.DamageReceived();
                
                TotalPoints += humanoid.GetDamageDone();
                TotalPoints += _weaponController.DamageReceived();
            }

            _saveLoadService.AddMoney(TotalMoney);
            _saveLoadService.ApplyTotalPoints(TotalPoints);
        }
        
        private void Init()
        {
            for (int i = 1; i < GeneralCountMembers; i++)
            {
                int countHumanoid = _saveLoadService.ReadAmountHumanoids(i);
                _membersBattles.Add(i, new InfoMemberBattle(0, 0));
            }
        }
    }
}