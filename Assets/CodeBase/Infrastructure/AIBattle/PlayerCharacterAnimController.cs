using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class PlayerCharacterAnimController : MonoCache
    {
        public readonly int Idle = Animator.StringToHash("Idle");
        public readonly int Run = Animator.StringToHash("IsRun");
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
        private Dictionary<int, float> _animInfo=new();

        private RuntimeAnimatorController animatorController;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            SetAnimInfo();
        }

        public void SetRandomAnimation()
        {  animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = animatorOverrideController;
            int randomIndex = UnityEngine.Random.Range(0, _walkAnimationClips.Length);
            animatorOverrideController["Walk"] = _walkAnimationClips[randomIndex];
        }

        public void OnShoot(bool satate)
        {
            _animator.SetBool(IsShoot,satate);
        }

        public void OnMove(bool satate)
        {
            _animator.SetBool(Run,satate);
        }
        
        public void OnReload(bool satate)
        {
            _animator.SetBool(Reload,satate);
        }
        
        public void OnIdle()
        {
            OnShoot( false);
            OnMove(false);
        }
        
        
        private void SetAnimInfo()
        {
            List<int>animHashNames = new();

            animHashNames.Add(IsShoot);
                animHashNames.Add(Reload);
                
            animatorController = _animator.runtimeAnimatorController;

            foreach (int name in animHashNames)
            {
                string animName = GetAnimatorParameterName(name);
                AnimationClip clip = GetAnimationClip(animName);
                float animationLength = clip.length;
                _animInfo.Add(name, animationLength);
            }
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



        public Dictionary<int, float> GetAnimInfo()
        {
            return _animInfo;
        }
    }
}