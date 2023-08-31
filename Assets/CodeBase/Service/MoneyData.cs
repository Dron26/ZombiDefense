using System;
using UnityEngine;

namespace Service
{
    [Serializable]
    public class MoneyData
    {
        public event Action MoneyChanged;
        public int Money;
        
        public int AllAmountMoney;
        public int AmountMoneyPerDay;

        public void AddMoney(int amountMoney)
        {
            Money += amountMoney;
            AllAmountMoney += amountMoney;
            AmountMoneyPerDay += amountMoney;
            MoneyChanged?.Invoke();
        }
        
        public void SpendMoney( int amountMoney)
        {
            Money -= Mathf.Clamp(amountMoney, 0, int.MaxValue);
            MoneyChanged?.Invoke();
        }


        public bool IsMoneyEnough(int price)
        {
            return Money >= price;
        }
    }
}