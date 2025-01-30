using System;
using UnityEngine;

namespace Services
{
    [Serializable]
    public class MoneyData
    {
        public int Money { get; set; }
        public int MoneyForEnemy { get; set; }
        public int AllAmountMoney { get; set; }
        public int TempMoney { get; set; }
    }
}