using System;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class TabButtonManager : MonoBehaviour
    {
        [field: SerializeField] public CanvasGroup TargetTab { get; private set; }
        [SerializeField] private Button _tabButton;

        public event EventHandler<OnClickEventArgs> OnClick;

        public void Initialize()
        {
            _tabButton.onClick.AddListener(Clicked);
        }

        public void Show()
        {
            _tabButton.interactable = false;
            TargetTab.alpha = 1;
            TargetTab.interactable = true;
            TargetTab.blocksRaycasts = true;
        }

        public void Hide()
        {
            _tabButton.interactable = true;
            TargetTab.alpha = 0;
            TargetTab.interactable = false;
            TargetTab.blocksRaycasts = false;
        }

        private void Clicked()
        {
            OnClick?.Invoke(this, new OnClickEventArgs(this));
        }
    }
}
