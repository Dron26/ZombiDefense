using UnityEngine;

namespace UI.Locations
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Locations/LocationData")]
    public class LocationData: ScriptableObject
    {
        public int Id;
        public bool IsTutorial;
        public bool IsLocked;
        public bool IsCompleted;


        public LocationData(int id, bool isTutorial, bool isLocked, bool isCompleted)
        {
            Id = id;
            IsTutorial = isTutorial;
            IsLocked = isLocked;
            IsCompleted = isCompleted;
        }
                    
        public void SetLock(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

                    
    }
}