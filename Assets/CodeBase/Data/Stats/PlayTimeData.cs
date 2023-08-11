using System;

namespace Data.Stats
{
    [Serializable]
    public class PlayTimeData
    {
        public float PlayTime;
        private float _targetPlayTime;

        public float Ratio => _targetPlayTime / PlayTime;

        public PlayTimeData(int targetPlayTime)
        {
            _targetPlayTime = targetPlayTime;
            Clear();
        }

        public void Add(float deltaTime) =>
            PlayTime += deltaTime;

        public void Clear() =>
            PlayTime = ConstantsData.Zero;

        public bool IsPlayTimeLessTarget() =>
            _targetPlayTime < PlayTime;

        public void Stop()
        {
        }
    }
}