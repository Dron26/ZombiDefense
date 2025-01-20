using UnityEngine;

namespace Data
{
    public abstract class AbilityData : ScriptableObject
    {
        public AbilityType AbilityType; 
        public bool IsEnabled = true;
    }
}