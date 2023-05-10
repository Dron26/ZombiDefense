using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class HashAnimator : MonoCache
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
    }
}