using System;
using Interface;
using Services;
using UnityEngine;

namespace Services.SaveLoad
{
    public class CurrencyHandler : ICurrencyHandler
    {
        private readonly MoneyData _moneyData;

        public event Action MoneyChanged;

        public CurrencyHandler(MoneyData moneyData)
        {
            _moneyData = moneyData ?? throw new ArgumentNullException(nameof(moneyData));
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

        public int FixTemporaryMoneyState()
        {
            _moneyData.TempMoney = _moneyData.Money;
            return _moneyData.TempMoney;
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