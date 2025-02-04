using System;
using System.Collections.Generic;
using Lean.Localization;
using Services.SaveLoad;

[Serializable]
public class UpgradeData
{
    private List<string> _purchasedUpgrades = new();
    public List<string> PurchasedUpgrades => _purchasedUpgrades;
}