using Data;
using Interface;
using Services.SaveLoad;

namespace Services
{
    public class UpgradeHandler : IUpgradeHandler
    {
        private GameData _gameData;
        private UpgradeInfo _upgradeInfo;
        public UpgradeHandler(UpgradeInfo upgradeInfo) => _upgradeInfo = upgradeInfo;

        public bool HasPurchasedUpgrade(string upgradeId) => _upgradeInfo.PurchasedUpgrades.Contains(upgradeId);
        public bool RefundUpgrade(string upgradeId, int refundAmount)
        {
            if (!HasPurchasedUpgrade(upgradeId))
                return false;

            RemovePurchasedUpgrade(upgradeId);
            AllServices.Container.Single<CurrencyHandler>().AddMoney(refundAmount); // Возвращаем деньги игроку
            return true;
        }
        
        public void AddPurchasedUpgrade(string upgradeId)
        {
            if (!_upgradeInfo.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeInfo.PurchasedUpgrades.Add(upgradeId);
            }
        }

        public void RemovePurchasedUpgrade(string upgradeId)
        {
            if (!_upgradeInfo.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeInfo.PurchasedUpgrades.Remove(upgradeId);
            }
        }
    }
}
//ок, давай вернемся к базе, нам нужны улучшения, их базовые классы  это будут//
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
// } они вносятся вusing System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]
// public class UpgradeData : ScriptableObject
// {
//     public string Id;
//     public string Name;
//     public string Description;
//     public int Cost;
//     public Sprite Icon;
//     public UpgradeType Type;
//
//     public float Value;  
//     public int UnlockId;
//     public List<UpgradeEffect> UpgradeEffect;
//}мы должны сначала итого получаем эффект и сам апгрейд содержащий его. дальше, мы выгружаем записанные апгрейды уже во время старта игры, нам нужен будет лоадер который если запуск первыы раз то он соберет все апгрейды UpgradeData создаст список этих базовах апгрейдов, если второй и последующие запуски то,  запросит из gameData  список текущих апгрейдов. возможно он будет заниматься только загрузкой их так как храниться они могут в UpgradeHandlere  используя UpgradeInfo  и им подобные. ок, мы загрузили данные, при покупке мы будем выбирать из списка апгрейд проверять доступность и тд ,брать его эффект  и применять его, вызывая событье и оповещая нужные компоненты и системы. соответственно нам нужны события под каждую покупку. ок дальше   мы применили событие в системе добавили в список сохранили. возможно при запуске стоит сразу применять параметры улучшений если какие то были открыты. 