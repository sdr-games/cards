using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.AbilitiesQueueModule.Views
{
    public class AbilitySlotView : Button
    {
        public void Initialize()
        {
            interactable = false;
        }

        public void SetIconSprite(Sprite sprite = null)
        {
            image.sprite = sprite;
            interactable = sprite != null;
        }
    }
}
