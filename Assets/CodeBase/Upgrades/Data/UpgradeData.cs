using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public int Cost;
    public Sprite Icon;
    public UpgradeType Type;
    public UpgradeGroupType GroupType;

    public int UnlockId;
    public List<float> UpgradesValue;
    public bool Lock;
}