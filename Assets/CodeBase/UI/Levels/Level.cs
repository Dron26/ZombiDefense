using System.Collections.Generic;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.Levels
{
    public class Level: MonoCache
    {
        [SerializeField] List<WaveData> _waveDatas;
        [SerializeField] public int Number;
        [SerializeField] public string Path;
        [SerializeField] public bool IsTutorial;
        
        public List<WaveData> GetWaveDataInfo()
        {
            return new List<WaveData>(_waveDatas);
        }
    }
}