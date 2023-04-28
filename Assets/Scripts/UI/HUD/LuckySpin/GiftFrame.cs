using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.HUD.LuckySpin
{
    public class GiftFrame : MonoCache
    {
        [SerializeField] private TMP_Text _nameGift;
        [SerializeField] private TMP_Text _descriptionGift;

        public int NumberGift { get; private set; }
        
        public void InitDescription(string nameGift, string descriptionGift, int numberGift)
        {
            _nameGift.text = nameGift;
            _descriptionGift.text = descriptionGift;

            NumberGift = numberGift;
        }
    }
}