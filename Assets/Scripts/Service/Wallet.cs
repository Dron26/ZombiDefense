using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;

namespace Service
{
    public class Wallet
    {
        private SaveLoad _saveLoad;
        public int Money => _money;
        private  int _money;

        public  Wallet(SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
            _money = _saveLoad.ReadAmountMoney();
        }
        
        public void AddMoney(int amountMoney)
        {
            _money += amountMoney;
            _saveLoad.ApplyMoney(_money);
        }
        
        public void SpendMoney(int amountSpendMoney)
        {
            _money -= Mathf.Clamp(amountSpendMoney, 0, int.MaxValue);
            _saveLoad.ApplyMoney(_money);
        }
        
        public bool CheckPossibilityBuy(int price)
        {
            return _money >= price;
        }
    }
}