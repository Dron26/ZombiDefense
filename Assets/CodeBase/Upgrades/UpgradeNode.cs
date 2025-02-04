using System.Collections.Generic;
using System.Linq;

public class UpgradeNode
{
    public IUpgrade Upgrade { get; }
    public List<UpgradeNode> Dependencies { get; }

    public UpgradeNode(IUpgrade upgrade)
    {
        Upgrade = upgrade;
        Dependencies = new List<UpgradeNode>();
    }

    public bool IsAvailable(List<IUpgrade> unlockedUpgrades)
    {
        return Dependencies.All(node => unlockedUpgrades.Contains(node.Upgrade));
    }
}