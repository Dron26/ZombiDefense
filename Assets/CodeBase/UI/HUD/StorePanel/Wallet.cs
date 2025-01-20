using System;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class Wallet:MonoCache
    {
        public int MoneyForEnemy { get; set; }
        public int AllAmountMoney { get; set; }

        public int TempMoney { get; set; }
        public event Action MoneyChanged;

        public void Initialize(SaveLoadService saveLoadService)
        {
            TempMoney = saveLoadService.FixMoneyState();
            saveLoadService.OnEnemyDeath += AddMoneyForKilledEnemy;
        }

        public void AddMoneyForKilledEnemy(Enemy enemy)
        {
            int amountMoney = enemy.GetPrice();
            TempMoney += amountMoney;
            AllAmountMoney += amountMoney;
            MoneyForEnemy += amountMoney;
            MoneyChanged?.Invoke();
        }

        public void AddMoney(int amountMoney)
        {
            TempMoney += amountMoney;
            AllAmountMoney += amountMoney;
            MoneyChanged?.Invoke();
        }

        public int ReadAmountMoney()
        {
            return TempMoney;
        }

        public void SpendMoney(int amountMoney)
        {
            TempMoney -= Mathf.Clamp(amountMoney, 0, int.MaxValue);
            MoneyChanged?.Invoke();
        }

        public bool IsMoneyEnough(int price)
        {
            return TempMoney >= price;
        }
    }
}