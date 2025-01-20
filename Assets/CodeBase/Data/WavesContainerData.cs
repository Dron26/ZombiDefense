using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WavesContainerData", menuName = "WavesContainerData")]
    public class WavesContainerData : ScriptableObject
    {
        public List<WaveData> GroupWaveData;
        public int LocationNumber;
    }
}