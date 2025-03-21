using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrade
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Cost { get; private set; }
    public Sprite Icon { get; private set; }
    public UpgradeType Type { get; private set; }
    public UpgradeGroupType GroupType { get; private set; }
    public int UnlockId { get; private set; }
    public bool Lock { get; private set; }
    public bool IsPurchased { get; private set; }
    public List<float> UpgradesValue;
    public Upgrade(UpgradeData data)
    {
        Id = data.Id;
        Name = data.Name;
        Description = data.Description;
        Cost = data.Cost;
        Icon = data.Icon;
        Type = data.Type;
        GroupType = data.GroupType;
        UnlockId = data.UnlockId;
        Lock = data.Lock;
        UpgradesValue = data.UpgradesValue;
    }

    public void SetLock(bool isLock)
    {
        Lock=isLock;
    }
    
    public void SetPurchased(bool isPurchased)
    {
        IsPurchased=isPurchased;
    }
}