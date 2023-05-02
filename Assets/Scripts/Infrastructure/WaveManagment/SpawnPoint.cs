using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Infrastructure.WaveManagment
{
    public class SpawnPoint:MonoCache
    {
        private int _number;
        private int _priority;
        private float _delayTime;

        public void  Initialize(int number, int priority)
        {
            _number = number;
            _priority = priority;
        }
        
    }
}