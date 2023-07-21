using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Upgrades
{
    public class UpgradeData : MonoCache
    {
        public string CharacterId => _characterId;
        public float MaxHealth => _maxHealth;
        public float MaxDamage => _maxDamage;
        private string _characterId;
        private float _maxHealth;
        private float _maxDamage;
        private List<int> _healthLevel;
        private List<int> _damageLevel;

        private void Initialize(List<int> healthLevel, List<int> damageLevel)
        {
            SetLevel(healthLevel, _healthLevel);
            SetLevel(damageLevel, _damageLevel);
            _maxHealth = _healthLevel.Count;
            _maxDamage = _damageLevel.Count;
        }

        private void SetLevel(List<int> getLevel, List<int> setLevel)
        {
            foreach (int level in getLevel)
            {
                setLevel.Add(level);
            }
        }

        public int GetHealthLevel(int index)
        {
            return _healthLevel[index];
        }

        private int GetDamageLevel(int index)
        {
            return _damageLevel[index];
        }
    }
}