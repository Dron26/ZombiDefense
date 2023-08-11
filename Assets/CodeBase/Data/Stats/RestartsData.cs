using System;

namespace Data.Stats
{
    [Serializable]
    public class RestartsData
    {
        public int Count;

        public RestartsData() =>
            Count = (int)ConstantsData.Zero;

        public void Increment() =>
            Count++;
    }
}