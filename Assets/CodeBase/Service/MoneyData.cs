using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;

namespace Service
{
    public class Wallet
    {
        private SaveLoadService.SaveLoadService _saveLoadService;
        public int Money => _money;
        private  int _money;

        public  Wallet(SaveLoadService.SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
            _money = _saveLoadService.ReadAmountMoney();
        }
        
        public void AddMoney(int amountMoney)
        {
            _money += amountMoney;
            _saveLoadService.AddMoney(_money);
        }
        
        public void SpendMoney( int amountMoney)
        {
            _money-=amountMoney; 
                _saveLoadService.SpendMoney(amountMoney);
                
        }
        
        public bool CheckPossibilityBuy(int price)
        {
            return _money >= price;
        }
    }
}