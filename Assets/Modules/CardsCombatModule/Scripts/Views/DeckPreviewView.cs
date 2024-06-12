using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class DeckPreviewView : MonoBehaviour
    {
        [SerializeField] private Image _backsideImage;

        public void Initialize(Sprite backside)
        {
            _backsideImage.sprite = backside;
        }

        private void OnEnable()
        {
            if (_backsideImage == null)
            {
                Debug.LogError("Backside Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
