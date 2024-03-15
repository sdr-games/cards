using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
{
    public abstract class BaseNodePresenter
    {
        public BaseNodePresenter() { }

        public abstract BaseNodeView GetNodeView();

        public abstract void Initialize(string name, Vector2 position);

        protected abstract void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e);
    }
}
