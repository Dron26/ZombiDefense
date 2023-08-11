using System.Collections.Generic;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.Levels
{
    public class LevelPoint: MonoCache
    {
        [SerializeField] List<WaveData> _waveDatas;

        public int Number;
        
        public List<WaveData> GetWaveDataInfo()
        {
            return _waveDatas;
        }
    }
}