using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.HUD.Store
{
    public class PricePanel: MonoCache
    {
        [SerializeField] private TMP_Text _priceText;

        public void SetInfo(int price)
        {
            _priceText.text = $"Price: ${price.ToString()}";
        }
    }
}