using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchView : MonoBehaviour
    {
        public static readonly Vector2 PADDING = new Vector2(50, 50);

        [SerializeField] private RectTransform _rectTransform;

        private UserInputController _userInputController;

        public event EventHandler BranchClicked;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        public void SetPositionAndSize(Vector2 position, Vector2 scale)
        {
            _rectTransform.sizeDelta = new Vector2(Screen.width / 2 + PADDING.x, Screen.height / 2 + PADDING.y);
            transform.localPosition = position;
            transform.localScale = scale;
        }

        public void SetRotation()
        {
            Vector3 relative = transform.InverseTransformPoint(transform.parent.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.RotateAround(transform.TransformPoint(_rectTransform.rect.center), Vector3.forward, 180 - angle);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject)
            {

            }
        }
    }
}
