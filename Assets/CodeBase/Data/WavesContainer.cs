using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Data
{
    public class WavesContainer : MonoCache
    {
        private WavesContainerData _wavesContainerData;

        public void Initialize(int locationNumber)
        {
            string path =AssetPaths.WavesContainerData + locationNumber;
            _wavesContainerData = Resources.Load<WavesContainerData>(path);
        }
    
        public WavesContainerData GetWavesContainer()
        {
            return _wavesContainerData;
        }
    }
}