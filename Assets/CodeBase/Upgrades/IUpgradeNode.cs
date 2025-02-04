using System.Collections.Generic;

public interface IUpgradeNode
{
    IUpgrade Upgrade { get; }
    List<IUpgradeNode> Dependencies { get; }
    bool IsAvailable(List<IUpgrade> unlockedUpgrades);
}