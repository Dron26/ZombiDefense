using System;
using UnityEngine;

namespace Services
{
    [Serializable]
    public class MoneyData
    {
        public event Action MoneyChanged;
        public int Money{ get; set;}
        public int MoneyForEnemy{ get; set;}
        public int AllAmountMoney{ get; set;}

        public int TempMoney{ get ; set;}
        public void AddMoney(int amountMoney)
        {
            Money = amountMoney;
            TempMoney += amountMoney;
            AllAmountMoney += amountMoney;
            MoneyChanged?.Invoke();
        }
        
        public void SpendMoney( int amountMoney)
        {
            TempMoney -= Mathf.Clamp(amountMoney, 0, int.MaxValue);
            MoneyChanged?.Invoke();
        }


        public bool IsMoneyEnough(int price)
        {
            return TempMoney >= price;
        }
        
        public void AddMoneyForKilledEnemy(int amountMoney)
        {
            TempMoney += amountMoney;
            AllAmountMoney += amountMoney;
            MoneyForEnemy += amountMoney;
            MoneyChanged?.Invoke();
        }

        public void ClearMoneyForKilledEnemy()
        {
            MoneyForEnemy = 0;
        }

        public int  FixTempMoneyState()
        {
            TempMoney=Money;
            return TempMoney;
        }
    }
}