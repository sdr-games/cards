using SDRGames.Whist.AnimationsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AnimationsModule
{
    public class AnimationsController : MonoBehaviour
    {
        private Animator _animator;
        private AnimationClip _impactAnimationClip;
        private AnimationClip _blockAnimationClip;
        private AnimationClip _dodgeAnimationClip;
        private AnimationClip _deathAnimationClip;

        public bool IsReady => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

        public void Initialize(Animator animator, CharacterAnimationsModel animations)
        {
            _animator = animator;
            _impactAnimationClip = animations.ImpactAnimationClip;
            _blockAnimationClip = animations.BlockAnimationClip;
            _dodgeAnimationClip = animations.DodgeAnimationClip;
            _deathAnimationClip = animations.DeathAnimationClip;
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

        public void PlayBlockAnimation()
        {
            PlayAnimation(_blockAnimationClip);
        }

        public void PlayDodgeAnimation()
        {
            PlayAnimation(_dodgeAnimationClip);
        }

        public void PlayDeathAnimation()
        {
            PlayAnimation(_deathAnimationClip);
        }
    }
}
