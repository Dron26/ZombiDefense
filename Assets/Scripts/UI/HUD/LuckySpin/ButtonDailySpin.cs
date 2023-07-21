using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Yandex;
using Service.ADS;
using Service.SaveLoadService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.LuckySpin
{
    public class ButtonDailySpin : MonoCache
    {
        [SerializeField] private TMP_Text _tmpCountSpins;
        [SerializeField] private SaveLoad _saveLoad;
        [SerializeField] private Image _iconReward;

        [SerializeField] private YandexAds _rewardAds;

        private const int RewardSpinsAds = 3;
        private const int MaxCountSpins = 3;

        private int _counterSpins = 3;

        private void Start()
        {
            _rewardAds.RewardShowed += GetSpin;
            _counterSpins = _saveLoad.GetCountSpins();
        }

        protected override void UpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;

            if (_counterSpins > MaxCountSpins)
                _counterSpins = MaxCountSpins;

            _iconReward.gameObject.SetActive(_counterSpins == 0);

            Draw();
        }

        protected override void OnDisabled()
        {
            _rewardAds.RewardShowed -= GetSpin;
            _saveLoad.SaveCountSpins(_counterSpins);
        }

        public bool CanSpin()
        {
            if (_counterSpins > 0)
            {
                _counterSpins--;
                Draw();
                return true;
            }

            Draw();
            return false;
        }

        public void TryGetSpin()
        {
            _rewardAds.ShowRewardedAd();
        }

        private void Draw() =>
            _tmpCountSpins.text = $"{_counterSpins.ToString()}/3";


        public void GetSpin()
        {
            _counterSpins += RewardSpinsAds;

            if (_counterSpins > MaxCountSpins)
                _counterSpins = MaxCountSpins;
        }
    }
}