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
    public UpgradeGroup Group { get; private set; }
    public float Value { get; private set; }
    public int UnlockId { get; private set; }
    public bool Lock { get; private set; }
    public Sprite IconUpgrade { get; private set; }
    public Sprite IconLock { get; private set; }
    public TextMeshProUGUI Info { get; private set; }
    public TextMeshProUGUI Price { get; private set; }
    public List<UpgradeEffect> UpgradeEffect;
    public Upgrade(UpgradeData data)
    {
        Id = data.Id;
        Name = data.Name;
        Description = data.Description;
        Cost = data.Cost;
        Icon = data.Icon;
        Type = data.Type;
        Group = data.Group;
        Value = data.Value;
        UnlockId = data.UnlockId;
        Lock = data.Lock;
        IconUpgrade = data.IconUpgrade;
        IconLock = data.IconLock;
        Info = data.Info;
        Price = data.Price;
        UpgradeEffect = data.UpgradeEffect;
    }
}