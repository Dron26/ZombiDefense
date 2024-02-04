using Characters.Humanoids;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Data.Upgrades
{
    public class UpgradeData : MonoCache
    {
        
        [SerializeField] private HumanoidType _humanoidType;
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private bool _isHaveGrenade;
        [SerializeField] private int  _countGrenade;
        [SerializeField] private int  _percentageCriticalShotChance;
        [SerializeField] private int  _shotRadius;
        [SerializeField] private int _price;
        public int Health => _health;
        public int Damage => _damage;
        public bool IsHaveGrenade => _isHaveGrenade;
        public int CountGrenade => _countGrenade;
        public int PercentageCriticalShotChance => _percentageCriticalShotChance;
        public int ShotRadius => _shotRadius;
        
        public HumanoidType HumanoidType => _humanoidType;
        public int Price => _price;

    }
}

// public string CharacterId => _characterId;
// public float MaxHealth => _maxHealth;
// public float MaxDamage => _maxDamage;
//         
// private string _characterId;
// private float _maxHealth;
// private float _maxDamage;
// private List<int> _healthLevel;
// private List<int> _damageLevel;
//
// private void Initialize(List<int> healthLevel, List<int> damageLevel)
// {
//     SetLevel(healthLevel, _healthLevel);
//     SetLevel(damageLevel, _damageLevel);
//     _maxHealth = _healthLevel.Count;
//     _maxDamage = _damageLevel.Count;
// }
//
// private void SetLevel(List<int> getLevel, List<int> setLevel)
// {
//     foreach (int level in getLevel)
//     {
//         setLevel.Add(level);
//     }
// }
//
// public int GetHealthLevel(int index)
// {
//     return _healthLevel[index];
// }
//
// private int GetDamageLevel(int index)
// {
//     return _damageLevel[index];
// }