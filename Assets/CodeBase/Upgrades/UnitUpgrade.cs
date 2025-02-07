using System;
using System.Collections.Generic;

public class UnitUpgrade : Upgrade
{
    public float StatIncrease { get; }

    public UnitUpgrade(string id, string name, int cost, float statIncrease)
        : base(id, name, cost)
    {
        StatIncrease = statIncrease;
    }

    public override bool CanPurchase(int playerMoney, List<Upgrade> unlockedUpgrades)
    {
        return playerMoney >= Cost;
    }

    public override void Apply()
    {
        Console.WriteLine($"{Name} применено! Улучшение на {StatIncrease * 100}%.");
    }

    public override UpgradeEffect GetUpgradeEffect()
    {
        throw new NotImplementedException();
    }
}