using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UI.BuyAndMerge;
using UnityEngine;

namespace UI.Resurse

{[DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        private StatsMoney _statsMoney;
        private SaveLoadService _saveLoadService;
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService= saveLoadService;
            _statsMoney = GetComponentInChildren<StatsMoney>();
            _statsMoney.Initialize(_saveLoadService.ReadAmountMoney());
            _saveLoadService.OnChangeMoney += OnChangeMoney;
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_saveLoadService.ReadAmountMoney());
    }
}