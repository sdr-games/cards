using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.Managers
{
    public class EnemyMeshManager : MonoBehaviour
    {
        //[SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Color32 _highlightColor;
        
        [SerializeField] private UserInputController _userInputController;
        private Color _baseColor;
        private bool _isSelected;

        public event EventHandler<MeshClickedEventArgs> MeshClicked;

        public void Initialize(UserInputController userInputController)
        {
            //_baseColor = _meshRenderer.material.color;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnScene += OnLeftMouseButtonClickedOnScene;
        }

        private void OnLeftMouseButtonClickedOnScene(object sender, LeftMouseButtonSceneClickEventArgs e)
        {
            _isSelected = !_isSelected;
            if (_isSelected)
            {
                //_meshRenderer.material.color = _highlightColor;
            }
            MeshClicked?.Invoke(this, new MeshClickedEventArgs(_isSelected));
        }

        public void OnMouseOver()
        {
            if(_isSelected)
            {
                return;
            }
            //_meshRenderer.material.color = _highlightColor;
        }

        public void OnMouseExit()
        {
            if(_isSelected)
            {
                return;
            }
            //_meshRenderer.material.color = _baseColor;
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnScene -= OnLeftMouseButtonClickedOnScene;
        }
    }
}
