
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using SDRGames.Whist.UserInputModule.ScriptableObjects;

namespace SDRGames.Whist.UserInputModule.Controller
{
    #nullable enable
    public class UserInputController : MonoBehaviour
    {
        [SerializeField] private LayerMask _sceneClickableLayers;

        [SerializeField] private KeyBindings[] _keyBindings;

        public event EventHandler? KeyboardAnyKeyHold;
        public event EventHandler? KeyboardAnyKeyPressed;
        public event EventHandler? KeyboardAnyKeyReleased;

        public event EventHandler<LeftMouseButtonUIClickEventArgs>? LeftMouseButtonClickedOnUI;
        public event EventHandler<LeftMouseButtonUIClickEventArgs>? LeftMouseButtonReleasedOnUI;

        public event EventHandler<LeftMouseButtonSceneClickEventArgs>? LeftMouseButtonHoldOnScene;
        public event EventHandler<LeftMouseButtonSceneClickEventArgs>? LeftMouseButtonClickedOnScene;
        public event EventHandler<LeftMouseButtonSceneClickEventArgs>? LeftMouseButtonReleasedOnScene;

        public event EventHandler<RightMouseButtonClickEventArgs>? RightMouseButtonClicked;
        public event EventHandler<RightMouseButtonClickEventArgs>? RightMouseButtonReleased;

        public event EventHandler<MiddleMouseButtonScrollEventArgs>? MiddleMouseButtonScrollStarted;
        public event EventHandler<MiddleMouseButtonScrollEventArgs>? MiddleMouseButtonScrollEnded;

        public bool KeyIsPressed(Key keyCode)
        {
            return FindKey(keyCode.ToString()).isPressed;
        }

        public bool KeyWasPressedThisFrame(Key keyCode)
        {
            return FindKey(keyCode.ToString()).wasPressedThisFrame;
        }

        public bool KeyWasReleasedThisFrame(Key keyCode)
        {
            return FindKey(keyCode.ToString()).wasReleasedThisFrame;
        }

        #region MonoBehaviours methods

        private void Update()
        {
            KeyboardButtonHoldListen();
            KeyboardButtonPressListen();
            KeyboardButtonReleaseListen();

            LeftMouseButtonClickListen();
            LeftMouseButtonHoldListen();
            LeftMouseButtonReleaseListen();

            RightMouseButtonClickListen();
            RightMouseButtonReleaseListen();

            MiddleMouseButtonScrollStartListen();
            MiddleMouseButtonScrollEndListen();
        }
        #endregion

        #region Keyboard listeners
        private void KeyboardButtonHoldListen()
        {
            foreach(KeyBindings keyBindings in _keyBindings)
            {
                foreach(Key key in keyBindings.GetKeys())
                {
                    if(KeyIsPressed(key))
                    {
                        KeyboardAnyKeyHold?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void KeyboardButtonPressListen()
        {
            foreach (KeyBindings keyBindings in _keyBindings)
            {
                foreach (Key key in keyBindings.GetKeys())
                {
                    if (KeyWasPressedThisFrame(key))
                    {
                        KeyboardAnyKeyPressed?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void KeyboardButtonReleaseListen()
        {
            foreach (KeyBindings keyBindings in _keyBindings)
            {
                foreach (Key key in keyBindings.GetKeys())
                {
                    if (KeyWasReleasedThisFrame(key))
                    {
                        KeyboardAnyKeyReleased?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
        #endregion

        #region LMB listeners
        private void LeftMouseButtonClickListen()
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame
                || Time.timeScale <= 0)
            {
                return;
            }

            // сначала ищем UI элемент, т.к. он должен срабатывать раньше, чем объекты сцены.
            GameObject? uiElement = TryToFindInteractibleUIElement();

            if(uiElement is not null)
            {
                LeftMouseButtonClickedOnUI?.Invoke(this, new LeftMouseButtonUIClickEventArgs(uiElement, Mouse.current.position.ReadValue()));
                return;
            }

            RaycastHit hitInfo = FindInteractiveObject();

            // эта проверка показывает, был ли найден хоть какой-то объект на сцене.
            if (hitInfo.distance > 0)
            {
                LeftMouseButtonClickedOnScene?.Invoke(this, new LeftMouseButtonSceneClickEventArgs(hitInfo.collider.gameObject, hitInfo.point));
            }
        }

        private void LeftMouseButtonHoldListen()
        {
            if (!Mouse.current.leftButton.isPressed
                || Time.timeScale <= 0)
            {
                return;
            }

            // сначала ищем UI элемент, т.к. он должен срабатывать раньше, чем объекты сцены.
            GameObject? uiElement = TryToFindInteractibleUIElement();

            if (uiElement is not null)
            {
                LeftMouseButtonClickedOnUI?.Invoke(this, new LeftMouseButtonUIClickEventArgs(uiElement, Mouse.current.position.ReadValue()));
                return;
            }

            RaycastHit hitInfo = FindInteractiveObject();

            // эта проверка показывает, был ли найден хоть какой-то объект на сцене.
            if (hitInfo.distance > 0)
            {
                LeftMouseButtonHoldOnScene?.Invoke(this, new LeftMouseButtonSceneClickEventArgs(hitInfo.collider.gameObject, hitInfo.point));
            }
        }

        private void LeftMouseButtonReleaseListen()
        {
            if (!Mouse.current.leftButton.wasReleasedThisFrame
                || Time.timeScale <= 0)
            {
                return;
            }

            // сначала ищем UI элемент, т.к. он должен срабатывать раньше, чем объекты сцены.
            GameObject? uiElement = TryToFindInteractibleUIElement();

            if (uiElement is not null)
            {
                LeftMouseButtonReleasedOnUI?.Invoke(this, new LeftMouseButtonUIClickEventArgs(uiElement, Mouse.current.position.ReadValue()));
                return;
            }

            RaycastHit hitInfo = FindInteractiveObject();
            // эта проверка показывает, был ли найден хоть какой-то объект на сцене.
            if (hitInfo.distance > 0)
            {
                LeftMouseButtonReleasedOnScene?.Invoke(this, new LeftMouseButtonSceneClickEventArgs(hitInfo.collider.gameObject, hitInfo.point));
            }
        }
        #endregion

        #region RMB listeners
        private void RightMouseButtonClickListen()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame
                || Time.timeScale <= 0)
            {
                return;
            }
            RightMouseButtonClicked?.Invoke(this, new RightMouseButtonClickEventArgs(Mouse.current.delta.ReadValue()));
        }

        private void RightMouseButtonReleaseListen()
        {
            if (!Mouse.current.rightButton.wasReleasedThisFrame
                || Time.timeScale <= 0)
            {
                return;
            }
            RightMouseButtonReleased?.Invoke(this, new RightMouseButtonClickEventArgs(Mouse.current.delta.ReadValue()));
        }
        #endregion

        #region MMB listeners
        private void MiddleMouseButtonScrollStartListen()
        {
            if(!Mouse.current.scroll.IsPressed()
                || Time.timeScale <= 0)
            {
                return;
            }
            MiddleMouseButtonScrollStarted?.Invoke(this, new MiddleMouseButtonScrollEventArgs(Mouse.current.scroll.ReadValue()));
        }

        private void MiddleMouseButtonScrollEndListen()
        {
            if (!Mouse.current.scroll.IsActuated()
                || Time.timeScale <= 0)
            {
                return;
            }
            MiddleMouseButtonScrollEnded?.Invoke(this, new MiddleMouseButtonScrollEventArgs(Mouse.current.scroll.ReadValue()));
        }
        #endregion

        private KeyControl FindKey(string keyCode)
        {
            ReadOnlyArray<KeyControl> allKeys = Keyboard.current.allKeys;
            for (int i = 0; i < allKeys.Count; i++)
            {
                if (string.Equals(allKeys[i].keyCode.ToString(), keyCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    return allKeys[i];
                }
            }
            return null;
        }

        private GameObject? TryToFindInteractibleUIElement()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            if(raycastResults.Count == 0)
            {
                return null;
            }

            if(raycastResults.Count > 1)
            {
                raycastResults.Sort((x, y) => y.depth.CompareTo(x.depth));
            }
            
            return raycastResults[0].gameObject;
        }

        private RaycastHit FindInteractiveObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray, 300f, _sceneClickableLayers);

            if (hits.Length == 0)
            {
                return new RaycastHit();
            }

            if (hits.Length == 1)
            {
                return hits[0];
            }

            List<RaycastHit> hitList = new List<RaycastHit>(hits);
            hitList.Sort((x, y) => x.distance.CompareTo(y.distance));
            foreach (RaycastHit hit in hitList)
            {
                var target = hit.collider;

                if (target.isTrigger)
                {
                    continue;
                }
                return hit;
            }
            return new RaycastHit();
        }
    }
}
