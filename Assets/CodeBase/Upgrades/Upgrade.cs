using System;
using System.Collections.Generic;

public abstract class Upgrade
{
    public string Id { get; }
    public string Name { get; }
    public int Cost { get; }

    protected Upgrade(string id, string name, int cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }

    public abstract bool CanPurchase(int playerMoney, List<Upgrade> unlockedUpgrades);
    public abstract void Apply();
}
