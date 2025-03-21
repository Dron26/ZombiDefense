using System;
using Services;

namespace Interface
{
    public interface ICurrencyHandler:IService
    {
        event Action MoneyChanged;
        public int GetCurrentMoney();
        void AddMoney(int amount);
        void SpendMoney(int amount);
        bool IsMoneyEnough(int price);
        void AddMoneyForKilledEnemy(int amount);
        void ClearMoneyForKilledEnemy();
        void Reset();
    }
}