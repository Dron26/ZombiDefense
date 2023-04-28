using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace HumanoidsUI.AbstractLevel
{
    public abstract class HumanoidUI : MonoCache
    {
        public abstract int GetLevel();
        public abstract int GetPrice();
    }
}