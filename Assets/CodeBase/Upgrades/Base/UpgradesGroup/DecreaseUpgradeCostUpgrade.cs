using UnityEngine;

[CreateAssetMenu(fileName = "DecreaseUpgradeCostUpgrade", menuName = "Upgrades/DecreaseUpgradeCost")]
public class DecreaseUpgradeCostUpgrade : UpgradeEffect
{
    public float CostReductionPercentage; // Например, 0.9 = -10%

    public override void Apply()
    {
        //Уменьшение стоимости улучшений:
    }
}