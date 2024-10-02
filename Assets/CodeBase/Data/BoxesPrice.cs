using System.Collections.Generic;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BoxesPrice", menuName = "BoxesPrice")]
    public class BoxesPrice : ScriptableObject
    {
        public List<BoxType> BoxType = new List<BoxType>();
        public List<int> Prices = new List<int>();
    }
}