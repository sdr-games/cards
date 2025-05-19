using SDRGames.Whist.AnimationsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AnimationsModule
{
    public class AnimationsController : MonoBehaviour
    {
        private Animator _animator;
        private AnimationClip _deathAnimationClip;
        private AnimationClip _impactAnimationClip;

        public bool IsReady => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        public void Initialize(Animator animator, CharacterAnimationsModel animations)
        {
            _animator = animator;
            _deathAnimationClip = animations.DeathAnimationClip;
            _impactAnimationClip = animations.ImpactAnimationClip;
        }

        public void PlayAnimation(AnimationClip animationClip)
        {
            if(animationClip == null)
            {
                return;
            }
            _animator.CrossFade(animationClip.name, 0.1f);
        }

        public void PlayImpactAnimation()
        {
            PlayAnimation(_impactAnimationClip);
        }

        public void PlayDeathAnimation()
        {
            PlayAnimation(_deathAnimationClip);
        }
    }
}
