using SDRGames.Whist.DialogueModule.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.Editor.Presenters
{
    public abstract class BaseNodePresenter
    {
        public BaseNodePresenter() { }

        public abstract BaseNodeView GetNodeView();

        public abstract void Initialize(string name, Vector2 position);

        protected abstract void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e);
    }
}
