using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class SelectableObject : MonoBehaviour
{
    [SerializeField] private UserInputController controller;
    [SerializeField] private HideableUIView mapUI;
    [SerializeField] private GameObject book;
    [SerializeField] private HideableUIView bookUI;
    [SerializeField] private UnityEvent action;

    private void OnEnable()
    {
        controller.LeftMouseButtonClickedOnScene += Controller_LeftMouseButtonClickedOnScene;
    }

    private void Controller_LeftMouseButtonClickedOnScene(object sender, LeftMouseButtonSceneClickEventArgs e)
    {
        if(gameObject.layer != LayerMask.NameToLayer("Selectable"))
        {
            return;
        }
        action.Invoke();
    }

    public void OpenMap()
    {
        mapUI.Show();
    }

    public void OpenBook()
    {
        book.SetActive(true);
    }

    public void OpenTalents()
    {
        SceneManager.LoadScene("TalentsScene");
    }

    public void OpenAlchemy()
    {

    }

    private void OnMouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Selectable");
    }

    private void OnMouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
