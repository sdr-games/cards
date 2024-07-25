using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class TabButtonsManager : MonoBehaviour
    {
        [SerializeField] private TabButtonManager[] _tabButtonManagers;
        [SerializeField] private ScrollRect _scrollRect;

        private TabButtonManager _activeTabButtonManager;

        public void Initialize()
        {
            foreach (TabButtonManager tab in _tabButtonManagers)
            {
                tab.Initialize();
                tab.OnClick += ShowTab;
            }
            _activeTabButtonManager = _tabButtonManagers[0];
            _activeTabButtonManager.Show();
        }

        private void ShowTab(object sender, OnClickEventArgs e)
        {
            _activeTabButtonManager.Hide();
            _scrollRect.content = (RectTransform)e.TabButtonManager.TargetTab.transform;
            e.TabButtonManager.Show();
            _activeTabButtonManager = e.TabButtonManager;
        }
    }
}
