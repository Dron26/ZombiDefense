using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;

namespace Service
{
    public class Wallet:MonoCache
    {
        private SaveLoad _saveLoad;
        public int Money => _money;
        private  int _money;

        public void Initialize(SaveLoad saveLoad)
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
            if (_money >= price)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}