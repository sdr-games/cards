using SDRGames.Whist.DialogueEditorModule.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Presenters
{
    public abstract class BaseNodePresenter
    {
        public BaseNodePresenter() { }

        public abstract BaseNodeView GetNodeView();

        public abstract void Initialize(string name, Vector2 position);

        protected abstract void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e);
    }
}
