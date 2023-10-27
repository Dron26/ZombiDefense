using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service;
using Service.SaveLoad;
using UnityEngine;

namespace UI.Resurse

{[DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        [SerializeField]private StatsMoney _statsMoney;
        private SaveLoadService _saveLoadService;
        
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService= saveLoadService;
            _statsMoney.Initialize(_saveLoadService.ReadAmountMoney()); 
            _saveLoadService.MoneyData.MoneyChanged += OnChangeMoney;
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_saveLoadService.ReadAmountMoney());
    }
}