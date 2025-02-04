using System.Collections.Generic;

public interface IUpgrade
{
    string Id { get; }
    int Cost { get; }
    void Apply();
    bool CanPurchase(int playerMoney, List<IUpgrade> unlockedUpgrades);
}