using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class RewardedAdsButton:MonoCache
    {
       // private AdsSetter _adsSetter;
        private Button _button;
        private void Initialize()
        {
            
        }
        
        private void Start()
        {
            _button = GetComponent<Button>();
            gameObject.SetActive(false);
        }
        
        public void OnClick()
        {
         //   _adsSetter.ShowRewardedAd();
            SetState(false);
        }

        private void SetState(bool isWork)
        {
            gameObject.SetActive(false);
        }
    }
}