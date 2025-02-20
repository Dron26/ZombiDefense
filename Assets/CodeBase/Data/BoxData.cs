using System.Collections.Generic;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "BoxData")]
    public class BoxData : ScriptableObject
    {
        public BoxType Type;
        public List<ItemType> ItemTypes;
        public List<int> Count;
        
        public void ApplyUpgrade( ItemType type, int count)
        {
            for (int i = 0; i < ItemTypes.Count; i++)
                {
                    if (ItemTypes[i] == type)
                    {
                        
                        Count[i] += count;
                    }
                }
        }
    }
}