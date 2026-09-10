using System.Linq;
using Enemies.AbstractEntity;
using Interface;
using Services;
using UI.HUD.StorePanel;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class AirstrikeController : MonoBehaviour
    {
        [SerializeField] private int _basePrice = 1500;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private ParticleSystem _explosionEffect;

        private Wallet _wallet;
        private IUpgradeTree _upgradeTree;
        private ISearchService _searchService;
        private int _damage;
        private int _price;

        public bool IsAvailable { get; private set; }
        public int Price => _price;
        public int Damage => _damage;
        public float ExplosionRadius => _explosionRadius;

        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
            _upgradeTree = AllServices.Container.Single<IUpgradeTree>();
            _searchService = AllServices.Container.Single<ISearchService>();

            UpdateUpgradeValues();
        }

        public bool TryExecute(Vector3 targetPosition)
        {
            UpdateUpgradeValues();

            if (!IsAvailable || !_wallet.IsMoneyEnough(_price))
            {
                return false;
            }

            _wallet.SpendMoney(_price);
            PlayEffect(targetPosition);

            foreach (Enemy enemy in _searchService.GetEntitiesInRange<Enemy>(targetPosition, _explosionRadius))
            {
                float distance = Vector3.Distance(enemy.transform.position, targetPosition);
                float damagePercent = Mathf.Clamp01(1f - distance / _explosionRadius);
                int damage = Mathf.RoundToInt(damagePercent * _damage);

                enemy.ApplyDamage(damage, ItemType.Airstrike);
            }

            return true;
        }

        private void UpdateUpgradeValues()
        {
            IsAvailable = TryGetLastPurchasedValue(
                UpgradeGroupType.AirStrikes,
                UpgradeType.IncreaseDamageAirstrike,
                out float damage);

            _damage = Mathf.RoundToInt(damage);

            TryGetLastPurchasedValue(
                UpgradeGroupType.AirStrikes,
                UpgradeType.DecreaseCostAirstrike,
                out float discount);

            _price = Mathf.RoundToInt(_basePrice * (100f - discount) / 100f);
        }

        private bool TryGetLastPurchasedValue(
            UpgradeGroupType groupType,
            UpgradeType type,
            out float value)
        {
            Upgrade upgrade = _upgradeTree.GetAllUpgrades()
                .Where(item => item.GroupType == groupType && item.Type == type)
                .Where(_upgradeTree.IsPurchased)
                .OrderBy(item => item.Id)
                .LastOrDefault();

            if (upgrade == null || upgrade.UpgradesValue.Count == 0)
            {
                value = 0;
                return false;
            }

            value = upgrade.UpgradesValue[0];
            return true;
        }

        private void PlayEffect(Vector3 targetPosition)
        {
            if (_explosionEffect == null)
            {
                return;
            }

            ParticleSystem effect = Instantiate(_explosionEffect, targetPosition, Quaternion.identity);
            effect.Play();
        }
    }
}
