using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private GameObject _sceneContent;
    [SerializeField] private Image _playerPin;
    [SerializeField] private PinView[] _pinViews;
    [SerializeField] private FloatingTextView _floatingTextView;

    private int _currentPinViewIndex;

    public void LoadMap(bool victory)
    {
        if(victory)
        {
            _pinViews[_currentPinViewIndex].MarkAsDone();
            _currentPinViewIndex++;
            _pinViews[_currentPinViewIndex].MarkAsActive();
        }
        _sceneContent.SetActive(true);
        SceneManager.UnloadSceneAsync("DemoScene");
    }

    private void OnEnable()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var pinView in _pinViews)
        {
            pinView.CTAButtonClicked += OnCTAButtonClick;
            pinView.PinClicked += OnPinClick;
            pinView.Initialize();
        }
        _currentPinViewIndex = 0;
        _pinViews[_currentPinViewIndex].MarkAsActive();
    }

    private void OnPinClick(object sender, EventArgs e)
    {
        PinView pinView = (PinView)sender;
        if(Vector2.Distance(_playerPin.rectTransform.anchoredPosition, pinView.RectTransform.anchoredPosition) > 0.1f)
        {
            _floatingTextView.SetText(pinView.Text);
            _floatingTextView.Show();
            StartCoroutine(MovePlayerPin(pinView));
        }
    }

    private void OnCTAButtonClick(object sender, bool isActive)
    {
        string sceneName = isActive ? "DemoScene" : "DemoTownScene";
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.completed += SceneLoaded;
    }

    private void SceneLoaded(AsyncOperation obj)
    {
        _sceneContent.SetActive(false);
    }

    private IEnumerator MovePlayerPin(PinView pinView)
    {
        Vector2 direction = (_playerPin.rectTransform.position - pinView.RectTransform.position).normalized * 0.1f;
        float topPosition = _floatingTextView.Text.preferredHeight;
        while (_floatingTextView.Text.rectTransform.anchoredPosition.y <= topPosition)
        { 
            yield return null;
            _playerPin.rectTransform.anchoredPosition += direction;
        }
    }
}
