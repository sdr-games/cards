using System;
using System.Linq;

using SDRGames.Whist.SettingsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class VideoSettingsManager : MonoBehaviour
    {
        public void ChangeWindowMode(DropdownChangeSettingsEventArgs e)
        {
            FullScreenMode fullScreenMode = (FullScreenMode)(e.Index + 1);
            Screen.fullScreenMode = fullScreenMode;
            if (fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.fullScreen = false;
                return;
            }
            Screen.fullScreen = true;
        }

        public void ChangeScreenResolution(DropdownChangeSettingsEventArgs e)
        {
            string[] resolutionSplitted = e.Value.Split('x');
            int width = int.Parse(resolutionSplitted[0]);
            int height = int.Parse(resolutionSplitted[1]);
            Screen.SetResolution(width, height, Screen.fullScreen);
        }

        public void ChangeRefreshRate(DropdownChangeSettingsEventArgs e)
        {
            RefreshRate refreshRate = Screen.resolutions.First(item => e.Value.Contains(Math.Round(item.refreshRateRatio.value).ToString())).refreshRateRatio;
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, refreshRate);
        }

        public void ChangeQuality(DropdownChangeSettingsEventArgs e)
        {
            QualitySettings.SetQualityLevel(e.Index);
        }

        public void ChangeBrightness(RangeChangeSettingsEventArgs e)
        {

        }
    }
}
