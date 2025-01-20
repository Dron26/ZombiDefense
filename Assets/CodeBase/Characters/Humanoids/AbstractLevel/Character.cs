using System;
using Common;
using Data.Upgrades;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Services.Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    public abstract class Character : Entity,IDamageable
    {
        public CharacterData CharacterData=> _characterData;
        public int Price => _characterData.Price;
        public bool IsMove => IsMoving;
        protected bool IsMoving;
        
        public override bool IsLife() => _isLife;
        private bool _isLife = true;
        public Sprite Sprite => _characterData.Sprite;
        public bool CanMove => _characterData.CanMove;

        private CharacterData _characterData;
        
        public int Health => _characterData.Health;
        public CharacterType Type => _characterData.Type;
        public event Action<Character> OnInitialize;
        public abstract void ApplyDamage(int damage);

        public abstract void SetUpgrade(UpgradeData upgrade, int level);

        public void SetPoint(WorkPoint workPoint) {}
        public void Initialize(CharacterData characterData)
        {
            _characterData = characterData;
            _isLife = true;
            Initialize();
            OnInitialize?.Invoke(this);
        }

        public abstract void Initialize();

        public abstract void SetAudioManager(AudioManager audioManager);
        public override void ApplyDamage(float damage, ItemType itemType)
        {
            int value = Convert.ToInt32(damage); 
            ApplyDamage(value);
        }

        protected virtual void Die()
        {
            _isLife = false;
        }
    }
}