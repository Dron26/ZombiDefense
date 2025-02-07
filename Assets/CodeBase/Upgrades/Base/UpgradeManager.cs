using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : IUpgradeManager
{
    private IUpgradeTree _upgradeTree;
    private List<IUpgrade> _unlockedUpgrades = new();
    private int _playerMoney;

    public UpgradeManager(IUpgradeTree upgradeTree,int startingMoney)
    {
        _upgradeTree = upgradeTree;
        _playerMoney = startingMoney;
    }

    public bool PurchaseUpgrade(string upgradeId)
    {
        if (_upgradeTree.CanPurchase(upgradeId, _unlockedUpgrades, _playerMoney))
        {
            var upgrade = _upgradeTree.GetUpgradeById(upgradeId);
            var upgradeEffect = upgrade.GetUpgradeEffect();
            upgradeEffect.Apply();
            _unlockedUpgrades.Add(upgrade);
            _playerMoney -= upgrade.Cost;
            return true;
        }
        return false;
    }

    public bool IsUnlocked(string upgradeId)
    {
        return _unlockedUpgrades.Exists(u => u.Id == upgradeId);
    }

    public int GetPlayerMoney()
    {
        return _playerMoney;
    }
}

public abstract class UpgradeEffect : ScriptableObject
{
    public abstract void Apply();
}
//
// [CreateAssetMenu(fileName = "IncreaseMovementSpeedUpgrade", menuName = "Upgrades/IncreaseMovementSpeed")]
// public class IncreaseMovementSpeedUpgrade : UpgradeEffect
// {
//     public float SpeedMultiplier; // Например, 1.2 = +20%
//
//     public override void Apply()
//     {
//         GameManager.Instance.IncreaseMovementSpeed(SpeedMultiplier);
//     }
// }
// Увеличение здоровья:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "IncreaseHealthUpgrade", menuName = "Upgrades/IncreaseHealth")]
// public class IncreaseHealthUpgrade : UpgradeEffect
// {
//     public float HealthMultiplier; // Например, 1.2 = +20%
//
//     public override void Apply()
//     {
//         GameManager.Instance.IncreaseHealth(HealthMultiplier);
//     }
// }
// Уменьшение стоимости улучшений:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "DecreaseUpgradeCostUpgrade", menuName = "Upgrades/DecreaseUpgradeCost")]
// public class DecreaseUpgradeCostUpgrade : UpgradeEffect
// {
//     public float CostReductionPercentage; // Например, 0.9 = -10%
//
//     public override void Apply()
//     {
//         GameManager.Instance.DecreaseUpgradeCost(CostReductionPercentage);
//     }
// }
// Увеличение шанса на критический урон:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "IncreaseCriticalChanceUpgrade", menuName = "Upgrades/IncreaseCriticalChance")]
// public class IncreaseCriticalChanceUpgrade : UpgradeEffect
// {
//     public float CriticalChanceIncrease; // Например, 0.2 = +20%
//
//     public override void Apply()
//     {
//         GameManager.Instance.IncreaseCriticalChance(CriticalChanceIncrease);
//     }
// }
// Разблокировка новых умений:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "UnlockAbilityUpgrade", menuName = "Upgrades/UnlockAbility")]
// public class UnlockAbilityUpgrade : UpgradeEffect
// {
//     public string AbilityId; 
//
//     public override void Apply()
//     {
//         GameManager.Instance.UnlockAbility(AbilityId);
//     }
// }
// Увеличение дохода от монет:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "IncreaseCoinRewardUpgrade", menuName = "Upgrades/IncreaseCoinReward")]
// public class IncreaseCoinRewardUpgrade : UpgradeEffect
// {
//     public float CoinRewardMultiplier; // Например, 1.5 = +50%
//
//     public override void Apply()
//     {
//         GameManager.Instance.IncreaseCoinReward(CoinRewardMultiplier);
//     }
// }
// Увеличение темпа перезарядки оружия:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "DecreaseReloadTimeUpgrade", menuName = "Upgrades/DecreaseReloadTime")]
// public class DecreaseReloadTimeUpgrade : UpgradeEffect
// {
//     public float ReloadTimeReduction; // Например, 0.8 = -20%
//
//     public override void Apply()
//     {
//         GameManager.Instance.DecreaseReloadTime(ReloadTimeReduction);
//     }
// }
// Автоматическое восстановление здоровья:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "HealthRegenerationUpgrade", menuName = "Upgrades/HealthRegeneration")]
// public class HealthRegenerationUpgrade : UpgradeEffect
// {
//     public float HealthPerSecond; // Например, 1.0 = восстанавливать 1 единицу здоровья в секунду
//
//     public override void Apply()
//     {
//         GameManager.Instance.StartHealthRegeneration(HealthPerSecond);
//     }
// }
// Увеличение дальности атаки:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "IncreaseAttackRangeUpgrade", menuName = "Upgrades/IncreaseAttackRange")]
// public class IncreaseAttackRangeUpgrade : UpgradeEffect
// {
//     public float AttackRangeMultiplier; // Например, 1.5 = +50%
//
//     public override void Apply()
//     {
//         GameManager.Instance.IncreaseAttackRange(AttackRangeMultiplier);
//     }
// }
// Разблокировка нового оружия:
//
// csharp
// Копировать
// Редактировать
// [CreateAssetMenu(fileName = "UnlockWeaponUpgrade", menuName = "Upgrades/UnlockWeapon")]
// public class UnlockWeaponUpgrade : UpgradeEffect
// {
//     public string WeaponId; 
//
//     public override void Apply()
//     {
//         GameManager.Instance.UnlockWeapon(WeaponId);
//     }
// }