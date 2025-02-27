using System.Collections;

using TMPro;

using UnityEngine;

namespace SDRGames.Whist.NotificationsModule
{
    public class NotificationController : MonoBehaviour
    {
        private const float FADING_TIMER = 1.0f;

        public static NotificationController Instance { get; private set; }

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _notificationText;

        private Coroutine _timerCoroutine;

        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

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
