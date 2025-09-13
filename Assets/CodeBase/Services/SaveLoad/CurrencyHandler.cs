using System;
using System.Collections.Generic;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;

namespace Services.SaveLoad
{
    public class CurrencyHandler : ICurrencyHandler
    {
        private readonly MoneyData _moneyData;
        private IGameEventBroadcaster _eventBroadcaster;
        public event Action MoneyChanged;
        private ISaveLoadService _saveLoadService;
       
        public CurrencyHandler(MoneyData moneyData)
        {
            _moneyData = moneyData;
            _eventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _saveLoadService= AllServices.Container.Single<ISaveLoadService>();
            if (_saveLoadService.GetGameData().IsFirstStart)
            {
                //AddMoney(_saveLoadService.GetGameData().RemoteConfig.GetMoneyAmount);
                AddMoney(500000);
                _saveLoadService.ChangeFirstStart();
            }
        }
        
        

        public int GetCurrentMoney() => _moneyData.AllAmountMoney;

        public void AddMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            _moneyData.AllAmountMoney += amount;
            _eventBroadcaster.InvokeOnMoneyChanged(_moneyData.AllAmountMoney);
            MoneyChanged?.Invoke();
        }

        public void SpendMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            _moneyData.AllAmountMoney -= Mathf.Clamp(amount, 0, int.MaxValue);
            MoneyChanged?.Invoke();
        }

        public bool IsMoneyEnough(int price)
        {
            return _moneyData.TempMoney >= price;
        }

        public void AddMoneyForKilledEnemy(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            _moneyData.TempMoney += amount;
            _moneyData.AllAmountMoney += amount;
            _moneyData.MoneyForEnemy += amount;
            MoneyChanged?.Invoke();
        }

        public void ClearMoneyForKilledEnemy()
        {
            _moneyData.MoneyForEnemy = 0;
        }
        
        public void Reset()
        {
            _moneyData.AllAmountMoney = 100;
            ClearMoneyForKilledEnemy();
            _moneyData.AllAmountMoney = 0;
            _moneyData.TempMoney = 0;
            MoneyChanged?.Invoke();
        }
    }
}