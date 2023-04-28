using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace EnemiesUI.AbstractEntity
{
    public abstract class Enemy : MonoCache
    {
        public abstract int GetLevel();
    }
}