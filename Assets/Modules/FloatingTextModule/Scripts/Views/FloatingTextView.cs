using System.Collections;

using SDRGames.Whist.FloatingTextModule.Managers;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEngine;

namespace SDRGames.Whist.FloatingTextModule.Views
{
    public class FloatingTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _floatingTextTMP;
        [SerializeField] private Animator _animator;

        private Coroutine _showCoroutine;

        public void Initialize(string text, Color textColor)
        {
            _floatingTextTMP.text = text;
            _floatingTextTMP.color = textColor;
        }

        public void Show()
        {
            if(_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
            }
            _animator.enabled = true;
            _showCoroutine = StartCoroutine(ShowCoroutine());
        }

        public IEnumerator ShowCoroutine()
        {
            yield return null;
            AnimatorClipInfo[] clipsInfo = _animator.GetCurrentAnimatorClipInfo(0);
            yield return new WaitForSeconds(clipsInfo[0].clip.length-0.05f);
            FloatingTextManager.ReturnToPool(this);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_floatingTextTMP), _floatingTextTMP);
            this.CheckFieldValueIsNotNull(nameof(_animator), _animator);
        }
    }
}
