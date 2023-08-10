using System;
using UnityEngine;

namespace Service
{
    public class MoneyData
    {
        public event Action MoneyChanged;
        public int Money => _money;
        private  int _money;

        public void AddMoney(int amountMoney)
        {
            _money += amountMoney;
        }
        
        public void SpendMoney( int amountMoney)
        {
            _money -= Mathf.Clamp(amountMoney, 0, int.MaxValue);
        }


        public bool IsMoneyEnough(int price)
        {
            return _money >= price;
        }
    }
}