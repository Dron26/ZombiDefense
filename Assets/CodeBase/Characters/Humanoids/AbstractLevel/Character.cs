using Common;
using Data.Upgrades;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    public abstract class Character : MonoCache
    {
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected ParticleSystem _ring;
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected int _price;
        public bool IsMove => _isMoving;
        protected bool _isMoving;
        public int Price => _price;
        [SerializeField] protected int _id;
        public int ID => _id;

        
        public bool IsLife { get; protected set; }
        public Sprite Sprite => _sprite;
        public bool CanMove => _canMove;
        public bool _canMove=false;
        protected virtual void Awake()
        {
            InitializeCharacter();
        }

        protected virtual void InitializeCharacter()
        {
            IsLife = true;

            if (transform.TryGetComponent(out Humanoid _))
            {
                _canMove = true;
            }

        }
        public int GetMaxHealth()
        {
            return _maxHealth;
        }
        public abstract void ApplyDamage(int damage);

       
        public string GetName() => Constants.GetName(_id);
        protected virtual void Die()
        {
            IsLife = false;
        }

        public abstract void SetUpgrade(UpgradeData upgrade, int level);
        
        public virtual void SetPoint(WorkPoint workPoint)
        {}
    }
}