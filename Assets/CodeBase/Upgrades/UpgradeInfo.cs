using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

[Serializable]
public class UpgradeInfo
{
    private List<string> _purchasedUpgrades = new();
    public List<string> PurchasedUpgrades => _purchasedUpgrades;
}