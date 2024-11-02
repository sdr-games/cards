using SDRGames.Whist.LocalizationModule.Models;

using TMPro;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardView : CardPreviewView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _costText;

        public void Initialize(Vector3 position, LocalizedString nameText, LocalizedString descriptionText, Sprite illustrationSprite, string costText)
        {
            transform.localPosition = position;
            SetRotation();
            transform.SetAsFirstSibling();
            base.Initialize(nameText, descriptionText, illustrationSprite);
            _costText.text = costText;
        }

        private void SetRotation()
        {
            Vector3 relative = transform.InverseTransformPoint(transform.parent.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.RotateAround(transform.TransformPoint(_rectTransform.rect.center), Vector3.forward, 180 - angle);
        }

        private void OnEnable()
        {
            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_costText == null)
            {
                Debug.LogError("Cost Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
