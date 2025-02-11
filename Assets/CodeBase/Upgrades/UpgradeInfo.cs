using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

[Serializable]
public class UpgradeInfo
{
    private List<int> _purchasedUpgrades = new();
    public List<int> PurchasedUpgrades => _purchasedUpgrades;
}