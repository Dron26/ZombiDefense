using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenerationUpgrade", menuName = "Upgrades/HealthRegeneration")]
public class HealthRegenerationUpgrade : UpgradeEffect
{
    public float HealthPerSecond; // Например, 1.0 = восстанавливать 1 единицу здоровья в секунду

    public override void Apply()
    {
        //Автоматическое восстановление здоровья:
    }
}