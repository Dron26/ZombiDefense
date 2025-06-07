using System;
using System.Linq;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class Wallet:MonoCache
    {
        public int MoneyForEnemy { get; set; }
        private IUpgradeTree _upgradeTree;

        public int TempMoney { get; set; }
        private int _defaultMoney = 300;
        public event Action MoneyChanged;
        private float _profitProcent=0;

        public void Initialize()
        {
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
            
            if (_profitProcent==0)
            {
                _profitProcent = 0.1f;
            }
            
            AddListener();
            TempMoney = _defaultMoney;
        }

        public void AddMoneyForKilledEnemy(Enemy enemy)
        {
            int amountMoney = enemy.GetPrice();
            TempMoney += amountMoney;
            MoneyForEnemy += amountMoney;
            MoneyChanged?.Invoke();
        }
        
        public  float GetAllProfit() => TempMoney+(MoneyForEnemy*0.1f);

        public void AddMoney(int amountMoney)
        {
            Debug.Log(TempMoney);
            int profit = amountMoney+(int) Mathf.Round(amountMoney * _profitProcent);
            TempMoney += profit;
            
            MoneyChanged?.Invoke();
        }

        public int ReadAmountMoney()
        {
            return TempMoney;
        }

        public void SpendMoney(int amountMoney)
        {
            TempMoney -= amountMoney;
            MoneyChanged?.Invoke();
        }

        public bool IsMoneyEnough(int price)
        {
            return TempMoney >= price;
        }
        
        private void  OnExitedLocation()
        {
           // AddMoney(Convert.ToInt32(TempMoney+(MoneyForEnemy*0.1f)));
            AllServices.Container.Single<ICurrencyHandler>().AddMoney(TempMoney);
        }
        
        private void AddListener()
        {
            AllServices.Container.Single<IGameEventBroadcaster>().OnEnemyDeath += AddMoneyForKilledEnemy;
            AllServices.Container.Single<IGameEventBroadcaster>().OnExitedLocation += OnExitedLocation;
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
            if (upgrades != null && upgrades.Count > 0&&upgrades[0]!=0f)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
    }
}