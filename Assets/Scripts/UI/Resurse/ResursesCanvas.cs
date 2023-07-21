using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UI.BuyAndMerge;
using UnityEngine;

namespace UI.Resurse

{[DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        private StatsMoney _statsMoney;
        private SaveLoad _saveLoad;
        public void Initialize(SaveLoad saveLoad)
        {
            _saveLoad= saveLoad;
            _statsMoney = GetComponentInChildren<StatsMoney>();
            _statsMoney.Initialize(_saveLoad.ReadAmountMoney());
            _saveLoad.OnChangeMoney += OnChangeMoney;
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_saveLoad.ReadAmountMoney());
    }
}