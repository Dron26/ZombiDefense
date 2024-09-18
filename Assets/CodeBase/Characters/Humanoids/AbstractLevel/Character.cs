using Common;
using Data.Upgrades;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    public abstract class Character : MonoCache
    {
        public CharacterData CharacterData=> _characterData;
        public int Price => _characterData.Price;
        public bool IsMove => IsMoving;
        protected bool IsMoving;
        public bool IsLife { get; protected set; }
        public Sprite Sprite => _characterData.Sprite;
        public bool CanMove => _characterData.CanMove;

        private CharacterData _characterData;
        
        public int Health => _characterData.Health;
        public CharacterType Type => _characterData.Type;

        public abstract void ApplyDamage(int damage);

        public abstract void SetUpgrade(UpgradeData upgrade, int level);

        public void SetPoint(WorkPoint workPoint) {}

        public void Initialize(CharacterData characterData) => _characterData=characterData;
    }
}