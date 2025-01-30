using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "WaveData")]
    public class WaveData : ScriptableObject
    {
        public List<Enemies.EnemyData> Enemies;
        public List<int> EnemyCount;
    }
}