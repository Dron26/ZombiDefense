using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class EnemyAnimController : MonoCache
    {
        public readonly int Idle = Animator.StringToHash("Idle");
        public readonly int Run = Animator.StringToHash("IsRun");
        public readonly int Walk = Animator.StringToHash("Walk");
        public readonly int IsСrawl = Animator.StringToHash("IsСrawl");
        public readonly int IsShoot = Animator.StringToHash("IsShoot");
        public readonly int IsAttack = Animator.StringToHash("IsAttack");
        public readonly int IsHit = Animator.StringToHash("IsHit");
        public readonly int Die = Animator.StringToHash("Die");
        public readonly int Threw = Animator.StringToHash("Threw");
        public readonly int GranadeTakeDamage = Animator.StringToHash("GranadeTakeDamage");
        public readonly int WakeUp = Animator.StringToHash("WakeUp");
        public AnimationClip _currentClip;
        
        [SerializeField] private AnimationClip[] _walkClips;
        [SerializeField] private AnimationClip[] _runClips;
        [SerializeField] private AnimationClip[] _attackClips;
        [SerializeField] private AnimationClip[] _throwClips;
        [SerializeField] private AnimationClip[] _jumpClips;
        [SerializeField] private AnimationClip[] _screamClips;
        [SerializeField] private AnimationClip[] _deathClips;
        [SerializeField] private AnimationClip[] _idleClips;
        [SerializeField] private AnimationClip[] _takeDamageClips;
        [SerializeField] private AnimationClip[] _takeGranadeClips;
        private Animator _animator;
        private AnimatorOverrideController _animatorOverrideController;
        private Dictionary<int, float> _animInfo = new();
        private RuntimeAnimatorController animatorController;


        public void Awake()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                enemy.OnEnemyEvent += HandleEnemyEvent;
                _animator = GetComponent<Animator>();
                //enemy.OnTakeDamage += OnHitFX;
                // enemy.OnDeath += OnDieFX;
                SetAnimInfo();
            }
        }

        private void SetAnimInfo()
        {
            List<int> animHashNames = new();
            animHashNames.Add(Walk);

            animatorController = _animator.runtimeAnimatorController;

            foreach (int name in animHashNames)
            {
                string animName = GetAnimatorParameterName(name);
                AnimationClip clip = GetAnimationClip(animName);
                float animationLength = clip.length;
                _animInfo.Add(name, animationLength);
            }

            SetRandomAnimation();
        }

        public void SetRandomAnimation()
        {
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;

            Dictionary<string, AnimationClip[]> animationClips = new Dictionary<string, AnimationClip[]>
            {
                { "Walk", _walkClips },
                { "Run", _runClips },
                { "Attack", _attackClips },
                { "TakeDamage", _takeDamageClips },
                // { "Jump", _jumpClips },
                //{ "Scream", _screamClips },
                { "Death", _deathClips },
                { "Idle", _idleClips }
            };

            foreach (var animationClip in animationClips)
            {
                int randomIndex = Random.Range(0, animationClip.Value.Length);
                _animatorOverrideController[animationClip.Key] = animationClip.Value[randomIndex];
                
                // if (animationClip.Key== "Attack")
                // {
                //     AnimationEvent animationEvent = new AnimationEvent();
                //     animationEvent.time = animationClip.Value[randomIndex].length-0.2f;
                //     animationEvent.functionName = "AttackEnd";
                //     animationClip.Value[randomIndex].AddEvent(animationEvent);
                // }
            }
        }

        private void HandleEnemyEvent(EnemyEventType eventType, WeaponType weaponType)
        {
            switch (eventType)
            {
                case EnemyEventType.Death:
                    OnDie();
                    break;
                case EnemyEventType.TakeDamage:
                    break;
                case EnemyEventType.TakeSmokerDamage:
                    break;
                case EnemyEventType.TakeSimpleWalkerDamage:
                    break;
                case EnemyEventType.TakeGranadeDamage:
                    OnGranadeDamage();
                    break;
            }
        }

        private void OnGranadeDamage()
        {
            //_animator.SetTrigger(Die);
        }

        private void OnDie()
        {
            _animator.SetTrigger(Die);
        }

        // поставить смерть и падение под землю  по событию в анимации


        public void OnAttack(bool isActive)
        {
            _animator.SetBool(IsAttack, isActive);
            
            if (isActive)
            {
                _currentClip= _animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            }
        }

        public void OnGranadeTakeDamage()
        {
            _animator.SetBool(GranadeTakeDamage, true);
        }


        private string GetAnimatorParameterName(int nameHash)
        {
            foreach (var parameter in _animator.parameters)
            {
                if (parameter.nameHash == nameHash)
                {
                    return parameter.name;
                }
            }

            return string.Empty;
        }

        private AnimationClip GetAnimationClip(string clipName)
        {
            foreach (var clip in animatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip;
                }
            }

            return null;
        }


        public AnimationClip[] GetWalkAnimationClips()
        {
            AnimationClip[] newClips = new AnimationClip[_walkClips.Length];

            for (int i = 0; i < _walkClips.Length; i++)
            {
                newClips[i] = _walkClips[i];
            }

            return newClips;
        }

        public Dictionary<int, float> GetAnimInfo()
        {
            return _animInfo;
        }

        public void SmokerDmage()
        {
            _animator.SetBool(Walk, false);
            _animator.SetTrigger(IsHit);
        }
    }
}