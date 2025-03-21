using System.Collections.Generic;
using System.Linq;

public class UpgradeNode
{
    public Upgrade Upgrade { get; }
    public UpgradeNode NextUpgrade { get; set; }
    public UpgradeNode(Upgrade upgrade)
    {
        Upgrade = upgrade;
    }

    public bool IsAvailable(HashSet<string> unlockedUpgrades)
    {
        string nodeKey = $"{Upgrade.GroupType}_{Upgrade.UnlockId}";

        foreach (var key in unlockedUpgrades)
        {
            if (key == nodeKey)
            {
                return true;
            }
        }

        return false;}
}