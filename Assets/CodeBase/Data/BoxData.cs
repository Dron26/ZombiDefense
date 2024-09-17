using System.Collections.Generic;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "BoxData")]
public class BoxData : ScriptableObject
{
    public BoxType Type;
    public List<ItemType> ItemTypes;
}