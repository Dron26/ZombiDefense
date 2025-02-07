using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseCoinRewardUpgrade", menuName = "Upgrades/IncreaseCoinReward")]
public class IncreaseCoinRewardUpgrade : UpgradeEffect
{
    public float CoinRewardMultiplier; // Например, 1.5 = +50%

    public override void Apply()
    {
        //Увеличение дохода от монет:
    }
}