using System.Collections;

using TMPro;

using UnityEngine;

public class FloatingTextView : MonoBehaviour
{
    private const float FADE_SPEED = 0.0015f;

    [field: SerializeField] public TextMeshProUGUI Text { get; private set; }

    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetText(string text)
    {
        Text.text = text;
    }

    public void Show()
    {
        StartCoroutine(FadeOut());
    }

    public void Hide()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        while (_canvasGroup.alpha < 1)
        {
            yield return null;
            _canvasGroup.alpha += FADE_SPEED;
        }
        StartCoroutine(StartFloating());
    }

    private IEnumerator FadeIn()
    {
        while (_canvasGroup.alpha > 0)
        {
            yield return null;
            _canvasGroup.alpha -= FADE_SPEED;
        }
    }

    private IEnumerator StartFloating()
    {
        float topPosition = Text.preferredHeight;
        //float speed = topPosition / 2000;
        while(Text.rectTransform.anchoredPosition.y <= topPosition)
        {
            yield return null;
            Text.rectTransform.anchoredPosition = new Vector2(Text.rectTransform.anchoredPosition.x, Text.rectTransform.anchoredPosition.y + 0.3f);
        }
        StartCoroutine(FadeIn());
    }
}
