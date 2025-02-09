using System.Collections.Generic;
using System.Linq;

public class UpgradeNode
{
    public Upgrade Upgrade { get; }
    public List<UpgradeNode> Dependencies { get; }

    public UpgradeNode(Upgrade upgrade)
    {
        Upgrade = upgrade;
        Dependencies = new List<UpgradeNode>();
    }

    public bool IsAvailable(List<Upgrade> unlockedUpgrades)
    {
        return Dependencies.All(node => unlockedUpgrades.Contains(node.Upgrade));
    }
}