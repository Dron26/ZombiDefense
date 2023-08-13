using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine.UI;

namespace LeaderBoard
{
    public class PlayerItem : MonoCache
    {
        public TextMeshProUGUI Rank;
        public RawImage Icon;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Score;
    }
}