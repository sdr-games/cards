using System;
using System.Collections;

using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.DialogueSystem.Views
{
    public class DialogueLinearView : MonoBehaviour
    {
        private readonly Color DEFAULT_COLOR = Color.white;

        [SerializeField] private Image _currentCharacterPortrair;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private float FadeSpeed = 1.0F;
        [SerializeField] private int RolloverCharacterSpread = 10;
        [SerializeField] private Color ColorTint;

        private UserInputController _userInputController;
        private Coroutine _appearanceCoroutine;
        private bool _textVisible;

        public event EventHandler Destroyed;

        public void Initialize(Sprite characterPortrairSprite, string characterName, string speechText, UserInputController userInputController)
        {
            _currentCharacterPortrair.sprite = characterPortrairSprite;
            _characterName.text = characterName;

            _text.text = speechText;
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonReleasedOnUI += OnLeftMouseButtonReleasedOnUI;

            ShowText();
        }

        public void ShowText(bool force = false)
        {
            if (force)
            {
                StopCoroutine(_appearanceCoroutine);
                _text.overrideColorTags = true;
                _text.color = DEFAULT_COLOR;
                _textVisible = true;
                return;
            }

            if(_appearanceCoroutine != null)
            {
                return;
            }
            _appearanceCoroutine = StartCoroutine(RevealTextCoroutine());
        }

        private void OnLeftMouseButtonReleasedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (_textVisible)
            {
                Destroy(this);
                return;
            }
            ShowText(true);
        }

        private IEnumerator RevealTextCoroutine()
        {
            _text.ForceMeshUpdate();
            TMP_TextInfo textInfo = _text.textInfo;
            Color32[] newVertexColors;
            int currentCharacter = 0;
            int startingCharacterRange = currentCharacter;
            bool isRangeMax = false;
            while (!isRangeMax)
            {
                int characterCount = textInfo.characterCount;
                // Spread should not exceed the number of characters.
                byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);
                for (int i = 0; i < currentCharacter + 1; i++)
                {
                    // Skip characters that are not visible
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;
                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                    // Get the current character's alpha value.
                    byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);
                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = alpha;
                    newVertexColors[vertexIndex + 1].a = alpha;
                    newVertexColors[vertexIndex + 2].a = alpha;
                    newVertexColors[vertexIndex + 3].a = alpha;
                    // Tint vertex colors
                    // Note: Vertex colors are Color32 so we need to cast to Color to multiply with tint which is Color.
                    newVertexColors[vertexIndex + 0] = (Color)newVertexColors[vertexIndex + 0] * ColorTint;
                    newVertexColors[vertexIndex + 1] = (Color)newVertexColors[vertexIndex + 1] * ColorTint;
                    newVertexColors[vertexIndex + 2] = (Color)newVertexColors[vertexIndex + 2] * ColorTint;
                    newVertexColors[vertexIndex + 3] = (Color)newVertexColors[vertexIndex + 3] * ColorTint;
                    if (alpha == 255)
                    {
                        startingCharacterRange += 1;
                        if (startingCharacterRange == characterCount)
                        {
                            isRangeMax = true; // Would end the coroutine.
                        }
                    }
                }
                // Upload the changed vertex colors to the Mesh.
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                if (currentCharacter + 1 < characterCount)
                    currentCharacter += 1;
                yield return new WaitForSeconds(0.25f - FadeSpeed * 0.01f);
            }
            _textVisible = true;
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonReleasedOnUI -= OnLeftMouseButtonReleasedOnUI;
            Destroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
