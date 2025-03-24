using System;
using System.Linq;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class Wallet:MonoCache
    {
        public int MoneyForEnemy { get; set; }
        public int AllAmountMoney { get; set; }
        private IUpgradeTree _upgradeTree;

        public int TempMoney { get; set; }
        private int _defaultMoney = 100000;
        public event Action MoneyChanged;
        private float _profitProcent=0;

        public void Initialize()
        {
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
            AddListener();
            TempMoney = _defaultMoney;
        }

        public void AddMoneyForKilledEnemy(Enemy enemy)
        {
            int amountMoney = enemy.GetPrice();
            int cost= 
            TempMoney += amountMoney;
            AllAmountMoney += amountMoney;
            MoneyForEnemy += amountMoney;
            MoneyChanged?.Invoke();
        }

        public void AddMoney(int amountMoney)
        {
            int profit = (int) Mathf.Round(amountMoney * _profitProcent);
            TempMoney += profit;
            AllAmountMoney += profit;
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
        
        private void SetUpgrades()
        {
            UpdateUpgradeValue(UpgradeGroupType.Profit,UpgradeType.IncreaseProfit, value => _profitProcent = value);
            UpdateUpgradeValue(UpgradeGroupType.CashLimit, UpgradeType.IncreaseStartCashLimit, value => TempMoney = value);
        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
    }
}