using System.Collections.Generic;
using Data;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;
using EnemyData = Enemies.EnemyData;

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
        public AnimationClip CurrentClip;
        
        [Space(10)] [Header("AnimationClip")] 
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
        
        [SerializeField] private AnimationClip _shieldbearerClip;
        
        private Animator _animator;
        private AnimatorOverrideController _animatorOverrideController;
        private Dictionary<int, float> _animInfo = new();
        private EnemyData _data;
        private Enemy _enemy;
        private bool _isThrower;
        private bool _isShieldbearer;
        private AnimationClip _tempClip;
        protected override void OnEnabled()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                _enemy = enemy;
                _enemy.OnEnemyEvent += HandleEnemyEvent;
                _enemy.OnInitialized += Initialize;
                _animator = GetComponent<Animator>();
            }
        }

        private void Initialize()
        {
            _data = _enemy.Data;
            _isThrower = _data.IsThrower;
            _isShieldbearer = _data.HasShield;
            //enemy.OnTakeDamage += OnHitFX;
            // enemy.OnDeath += OnDieFX;
            
            SetSkin();
            //SetAnimInfo();
            SetAnimation();
        }

        
        // private void SetAnimInfo()
        // {
        //     List<int> animHashNames = new() { Walk };
        //
        //     foreach (int name in animHashNames)
        //     {
        //         string animName = GetAnimatorParameterName(name);
        //         AnimationClip clip = GetAnimationClip(animName);
        //         if (clip != null)
        //         {
        //             _animInfo.Add(name, clip.length);
        //         }
        //     }
        // }

        private void SetSkin()
        {
            GroupContainer groupContainer = GetComponentInChildren<GroupContainer>();
            
            SkinGroup[] skinsGroup = groupContainer.GetComponentsInChildren<SkinGroup>();

            if (!_data.IsSpecial&&!_data.IsPolice)
            {
                foreach (SkinGroup group in skinsGroup)
                {
                    if (!group.IsSpecial)
                    {
                        group.Initialize(0);
                    }
                }
            }
            else
            {
                SetSpecial( groupContainer);
            }
        }

        private void SetSpecial(GroupContainer groupContainer)
        {
            if (_data.IsPolice)
            {
                int index = (Random.Range(0, groupContainer.PoliceBody.transform.childCount));
                groupContainer.PoliceHead.Initialize(index);
                groupContainer.PoliceBody.Initialize(index);
                groupContainer.PoliceLeg.Initialize(index);
            }
            else if (_data.IsSpecial)    
            {
                int index = (Random.Range(0, groupContainer.SpecialHead.transform.childCount));
                groupContainer.SpecialHead.Initialize(index);
                groupContainer.SpecialBody.Initialize(index);
                groupContainer.SpecialLeg.Initialize(index);
            }
        }

        public void SetAnimation()
        {
            _animator.runtimeAnimatorController = _data.CharacterController;
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

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
            }

            if (_isThrower)
            {
                _animatorOverrideController["Attack"] = animationClips["Attack"][0];
            }

            _animator.runtimeAnimatorController = _animatorOverrideController;
        }

        private void HandleEnemyEvent(EnemyEventType eventType, ItemType itemType)
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

        public AnimationClip GetAnimationClip(int  id)
        {
            return _animatorOverrideController.animationClips[id];
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

        public void SmokerDmage()
        {
            _animator.SetBool(Walk, false);
            _animator.SetTrigger(IsHit);
        }

        public void WasShieldShattered()
        {
            _animatorOverrideController["Walk"] = _tempClip;
        }
    }
}