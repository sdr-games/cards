using UnityEngine;

namespace SDRGames.Whist.AnimationsModule
{
    public class AnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public bool IsReady => _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle");

        public void Initialize(Animator animator)
        {
            _animator = animator;
        }

        public void PlayAnimation(AnimationClip animationClip)
        {
            if(animationClip == null)
            {
                return;
            }
            _animator.Play(animationClip.name);
        }
    }
}
