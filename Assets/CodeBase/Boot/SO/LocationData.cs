using UnityEngine;
using UnityEngine.UI;

namespace Boot.SO
{
    [CreateAssetMenu(menuName = "LocationData")]
    public class LocationData : ScriptableObject
    {
        public int Id;
        public int _maxEnemyOnLevel;
        public bool IsTutorial;
        public bool IsLocked=true;
        public bool IsCompleted;
        private Button _button;
    }
}