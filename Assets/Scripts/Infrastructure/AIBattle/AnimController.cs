using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEditor.Animations;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class AnimController : MonoCache
    {
        public readonly int Run = Animator.StringToHash("Run");
        public readonly int Walk = Animator.StringToHash("Walk");
        public readonly int IsСrawl = Animator.StringToHash("IsСrawl");
        public readonly int IsShoot = Animator.StringToHash("IsShoot");
        public readonly int Attack = Animator.StringToHash("Attack");
        public readonly int Reload = Animator.StringToHash("Reload");
        public readonly int IsHit = Animator.StringToHash("IsHit");
        public readonly int IsThrewGrenade = Animator.StringToHash("IsThrewGrenade");
        public readonly int Die = Animator.StringToHash("Die");

        [SerializeField] private  AnimationClip[] _walkAnimationClips;
        [SerializeField] private  AnimationClip[] _screamAnimationClips;
    
        private Animator _animator;
        private AnimatorOverrideController animatorOverrideController;
        private int weaponIndex;
        private Dictionary<int, float> _animInfo=new();
        
        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            weaponIndex = 0;
           
            
            SetAnimInfo();
        }

        public void SetRandomAnimation()
        {  animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = animatorOverrideController;
            int randomIndex = UnityEngine.Random.Range(0, _walkAnimationClips.Length);
            animatorOverrideController["Walk"] = _walkAnimationClips[randomIndex];
        }
        
        private void SetAnimInfo()
        {
            List<int>animHashNames = new();
            
            if (TryGetComponent(out Enemy enemy))
            {
                animHashNames.Add(Walk);
            }else
            {
                animHashNames.Add(IsShoot);
                animHashNames.Add(Reload);
            }
            

            UnityEditor.Animations.AnimatorController animatorController = _animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;


            foreach (int name in animHashNames)
            {
                string animName = animatorController.parameters.FirstOrDefault(p => p.nameHash == name)?.name;
                AnimationClip clip = animatorController.animationClips.FirstOrDefault(x => x.name == animName); 
                float animationLength = clip.length;
                _animInfo.Add(name, animationLength);
            }
        }

        public AnimationClip[] GetWalkAnimationClips()
        {
            
            AnimationClip[] newClips = new AnimationClip[_walkAnimationClips.Length];

            for (int i = 0; i < _walkAnimationClips.Length; i++)
            {
                newClips[i] = _walkAnimationClips[i];
            }
            return newClips;
        }
        
        public AnimationClip[] GetScreamAnimationClips()
        {
            
            AnimationClip[] newClips = new AnimationClip[_screamAnimationClips.Length];

            for (int i = 0; i < _screamAnimationClips.Length; i++)
            {
                newClips[i] = _screamAnimationClips[i];
            }
            return newClips;
        }
        
        
        
        public Dictionary<int, float> GetAnimInfo()
        {
            return _animInfo;
        }

    }
}