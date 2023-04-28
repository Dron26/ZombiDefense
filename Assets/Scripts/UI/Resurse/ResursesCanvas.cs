using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UI.BuyAndMerge;
using UI.BuyAndMerge.Merge;
using UnityEngine;

namespace UI.Resurse

{[DisallowMultipleComponent]
    public class ResursesCanvas : MonoCache
    {
        private StatsMoney _statsMoney;
        private SaveLoad _saveLoad;
        private CharacterSeller _characterSeller;
        public void Initialize(SaveLoad saveLoad, CharacterSeller characterSeller)
        {
            _saveLoad= saveLoad;
            _characterSeller = characterSeller;
            _characterSeller.ChangeMoney += OnChangeMoney;
            _statsMoney = GetComponentInChildren<StatsMoney>();
            _statsMoney.Initialize(_saveLoad.ReadAmountMoney());
        }

        private void OnChangeMoney() => _statsMoney.SetMoney(_saveLoad.ReadAmountMoney());

        protected override void OnDisabled()
        {
            _characterSeller.ChangeMoney -= OnChangeMoney;
        }
    }
}