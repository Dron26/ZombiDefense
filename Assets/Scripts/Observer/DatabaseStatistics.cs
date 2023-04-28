using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Service.SaveLoadService;
using TMPro;
using UnityEngine;

namespace Observer
{
    public class DatabaseStatistics : MonoCache
    {
        [SerializeField] private SaveLoad _saveLoad;
        
        private const int GeneralCountMembers = 12;

        private readonly Dictionary<int, InfoMemberBattle> _membersBattles = new();
        
        public int TotalMoney { get; private set; }
        public int TotalPoints { get; private set; }
        
        public Dictionary<int, InfoMemberBattle> GetMembersBattle() =>
            _membersBattles;
        
        public void SetDataBase(Factory factory)
        {
            Init();

            foreach (Humanoid humanoid in factory.GetAllHumanoids)
            { 
                _membersBattles[humanoid.GetLevel()].DamageDone += humanoid.GetDamageDone();
                _membersBattles[humanoid.GetLevel()].DamageReceived += humanoid.DamageReceived();
                _membersBattles[humanoid.GetLevel()].TotalPoints += humanoid.TotalPoints();
                
                TotalMoney += humanoid.GetDamageDone();
                TotalMoney += humanoid.DamageReceived();
                TotalMoney += humanoid.TotalPoints();
                
                TotalPoints += humanoid.GetDamageDone();
                TotalPoints += humanoid.DamageReceived();
                TotalPoints += humanoid.TotalPoints();
            }

            _saveLoad.ApplyMoney(TotalMoney);
            _saveLoad.ApplyTotalPoints(TotalPoints);
        }
        
        private void Init()
        {
            for (int i = 1; i < GeneralCountMembers; i++)
            {
                int countHumanoid = _saveLoad.ReadAmountHumanoids(i);
                _membersBattles.Add(i, new InfoMemberBattle(0, 0, 0));
            }
        }
    }
}