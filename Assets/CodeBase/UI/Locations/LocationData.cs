using UnityEngine;

namespace UI.Locations
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Locations/LocationData")]
    public class LocationData : ScriptableObject
    {
        [Header("Base Settings")]
        public int Id;
        public bool IsTutorial;
        public bool IsLocked;
        public bool IsCompleted;

        [Header("Wave Settings")]
        public int BaseZombieHealth;
        public int BaseReward; 
        public int WaveCount; 
        
    }
}