using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Infrastructure.AIBattle
{
    public class GranadeData:MonoCache
    {
        public float minDistance=2f; 
        public float maxDistance=15f;
        public float minSpeed=4f;
        public float maxSpeed=11f;
    }
}