using Humanoids.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using Infrastructure.Weapon;
using UnityEngine;

namespace Humanoids.People
{
    public class Soldier : PeopleMen
    {
        private const int Level = 1;
        private const int Price = 1;
        private const int Damage = 15;
        

        private readonly float _minHealth = 0f;
        private readonly float _maxHealth = 10f;
        
        private bool _isLife = true;

        private float _health = 10f;

        private HashAnimator _hashAnimator;
        private Animator _animator;
        private FXController _fxController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
        }

        // protected void void SetAttacments()
        // {
        //     WeaponController weaponController = GetComponent<WeaponController>();
        //     weaponController.Initialize(Weapons);
        //     
        //     foreach (GameObject items in humanoidData.PrefabCharacterItems)
        //     {
        //         GameObject item = Instantiate(items, transform.position, Quaternion.identity);
        //     }
        // }

        public override float GetHealth()
        {
            return _health;
        }

        public override bool IsLife() => 
            _isLife;

        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;

       

        public override void ApplyDamage(int getDamage)
        {
            if (_health <= 0)
            {
                _animator.SetTrigger(_hashAnimator.Die);
                _fxController.OnDieFX();
                _isLife = false;
            }
            
            _fxController.OnHitFX();
            _animator.SetTrigger(_hashAnimator.IsHit);
            _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
        }
        
        public override int GetDamageDone() => 
            (int)Mathf.Round(_maxHealth - _health);
    }
}