using System.Collections.Generic;
using Infrastructure.AIBattle.AdditionalEquipment;
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
    }
}