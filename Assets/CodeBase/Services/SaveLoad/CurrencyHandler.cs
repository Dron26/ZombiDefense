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

        public event Action MoneyChanged;
        private const int InitialMoneyAmount = 100000;
        public CurrencyHandler(MoneyData moneyData)
        {
            _moneyData = moneyData;
            
            if (!AllServices.Container.Single<ISaveLoadService>().GetGameData().IsFirstStart)
            {
                
                AddMoney(InitialMoneyAmount);
                AllServices.Container.Single<ISaveLoadService>().ChangeFirstStart();
            }

            AllServices.Container.Single<ISaveLoadService>().Save();
        }
        
        

        public int GetCurrentMoney() => _moneyData.AllAmountMoney;

        public void AddMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            _moneyData.Money = amount;
            _moneyData.TempMoney += amount;
            _moneyData.AllAmountMoney += amount;
            MoneyChanged?.Invoke();
        }

        public void SpendMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            _moneyData.TempMoney -= Mathf.Clamp(amount, 0, int.MaxValue);
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
            _moneyData.Money = 100;
            ClearMoneyForKilledEnemy();
            _moneyData.AllAmountMoney = 0;
            _moneyData.TempMoney = 0;
            MoneyChanged?.Invoke();
        }
    }
}