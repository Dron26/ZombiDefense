using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.HUD.StorePanel;
using UnityEngine;

namespace UI.Resurse
{
    [DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        [SerializeField] private StatsMoney _statsMoney;
        private Wallet _wallet;

        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
            _statsMoney.Initialize(_wallet.ReadAmountMoney());
            _wallet.OnMoneyChanged += OnChangeMoney;
            _wallet.OnEndMoney += OnEndMoney;
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_wallet.ReadAmountMoney());

        private void OnEndMoney()
        {
            _statsMoney.SetMoney(_wallet.ReadAmountMoney());
            _statsMoney.ShowNotEnoughMoney(); 
        }

        protected  void OnDestroy()
        {
            if (_wallet != null)
            {
                _wallet.OnMoneyChanged -= OnChangeMoney;
                _wallet.OnEndMoney -= OnEndMoney;
            }
        }
    }
}