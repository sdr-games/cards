using System.Collections;

using TMPro;

using UnityEngine;

namespace SDRGames.Whist.NotificationsModule
{
    public class Notification : MonoBehaviour
    {
        private const float FADING_TIMER = 1.0f;

        public static Notification Instance { get; private set; }

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _notificationText;

        private Coroutine _timerCoroutine;

        public static void Show(string message)
        {
            Instance._notificationText.text = message;
            Instance._canvasGroup.alpha = 1;
            if (Instance._timerCoroutine != null)
            {
                Instance.StopCoroutine(Instance._timerCoroutine);
            }
            Instance._timerCoroutine = Instance.StartCoroutine(Instance.StartTimer());
        }

        private void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private IEnumerator StartTimer()
        {
            yield return null;
            float timer = 0;
            while(timer < FADING_TIMER)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            while(_canvasGroup.alpha > 0)
            {
                yield return null;
                _canvasGroup.alpha -= Time.deltaTime;
            }
        }
    }
}
