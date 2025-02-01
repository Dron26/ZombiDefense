using System;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class Wallet:MonoCache
    {
        public int MoneyForEnemy { get; set; }
        public int AllAmountMoney { get; set; }

        public int TempMoney { get; set; }
        public event Action MoneyChanged;

        public void Initialize()
        {
            TempMoney = AllServices.Container.Single<ICurrencyHandler>().FixTemporaryMoneyState();
            AddListener();
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
        
        private void AddListener()
        {
            AllServices.Container.Single<IGameEventBroadcaster>().OnEnemyDeath += AddMoneyForKilledEnemy;
        }

        protected override void OnDisable()
        {
            RemoveListener();
        }

        private void RemoveListener()
        {
            AllServices.Container.Single<IGameEventBroadcaster>().OnEnemyDeath -= AddMoneyForKilledEnemy;
        }
    }
}