using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service;
using Service.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;

namespace UI.Resurse

{[DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        [SerializeField]private StatsMoney _statsMoney;
        private Wallet _wallet;
        
        public void Initialize(Wallet wallet)
        {
            _wallet= wallet;
            _statsMoney.Initialize(_wallet.ReadAmountMoney()); 
            _wallet.MoneyChanged += OnChangeMoney;
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_wallet.ReadAmountMoney());
    }
}