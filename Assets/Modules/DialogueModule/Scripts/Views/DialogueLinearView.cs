using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.DialogueSystem.Views
{
    public class DialogueLinearView : MonoBehaviour
    {
        [SerializeField] private Image _currentCharacterPortrair;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private float FadeSpeed = 1.0F;
        [SerializeField] private int RolloverCharacterSpread = 10;
        [SerializeField] private Color ColorTint;

        private Coroutine _appearanceCoroutine;

        private void Start()
        {
            Initialize(null, "Valior", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
        }

        public void Initialize(Sprite characterPortrairSprite, string characterName, string speechText)
        {
            _currentCharacterPortrair.sprite = characterPortrairSprite;
            _characterName.text = characterName;

            _text.text = speechText;

            ShowText();
        }

        public void ShowText(bool force = false)
        {
            if (force)
            {
                StopCoroutine(_appearanceCoroutine);
                return;
            }

            if(_appearanceCoroutine != null)
            {
                return;
            }
            _appearanceCoroutine = StartCoroutine(RevealTextCoroutine());
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
                for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
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
                        //startingCharacterRange += 1;
                        if (startingCharacterRange == characterCount)
                        {
                            // Update mesh vertex data one last time.
                            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                            yield return new WaitForSeconds(0.25f - FadeSpeed * 0.01f);
                            // Reset the text object back to original state.
                            _text.ForceMeshUpdate();
                            yield return new WaitForSeconds(0.25f - FadeSpeed * 0.01f);
                            // Reset our counters.
                            currentCharacter = 0;
                            startingCharacterRange = 0;
                            //isRangeMax = true; // Would end the coroutine.
                        }
                    }
                }
                // Upload the changed vertex colors to the Mesh.
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                if (currentCharacter + 1 < characterCount)
                    currentCharacter += 1;
                yield return new WaitForSeconds(0.25f - FadeSpeed * 0.01f);
            }
        }
    }
}
