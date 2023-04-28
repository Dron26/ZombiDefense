using System;
using UnityEngine;

namespace UI.HUD.LuckySpin
{
    [Serializable]
    public class GiftUpCard
    {
        public Sprite IconGift;
        public string NameGift;

        public GiftUpCard(Sprite iconGift, string nameGift)
        {
            IconGift = iconGift;
            NameGift = nameGift;
        }
    }
}