using System;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class RelationshipRemovedEventArgs : EventArgs
    {
        public BaseNodeView InputNodeView { get; private set; }

        public RelationshipRemovedEventArgs(BaseNodeView inputNodeView)
        {
            InputNodeView = inputNodeView;
        }
    }
}