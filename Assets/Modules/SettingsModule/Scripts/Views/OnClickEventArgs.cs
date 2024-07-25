using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class OnClickEventArgs
    {
        public TabButtonManager TabButtonManager { get; private set; }

        public OnClickEventArgs(TabButtonManager tabButtonManager)
        {
            TabButtonManager = tabButtonManager;
        }
    }
}