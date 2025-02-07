using UnityEngine;

[CreateAssetMenu(fileName = "DecreaseReloadTimeUpgrade", menuName = "Upgrades/DecreaseReloadTime")]
public class DecreaseReloadTimeUpgrade : UpgradeEffect
{
    public float ReloadTimeReduction; // Например, 0.8 = -20%

    public override void Apply()
    {
        //Увеличение темпа перезарядки оружия:
    }
}