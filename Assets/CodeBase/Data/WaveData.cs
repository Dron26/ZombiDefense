using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "WaveData")]
    public class WaveData : ScriptableObject
    {
        public List<EnemyData> Enemies;
        public List<int> EnemyCount;
    }
}