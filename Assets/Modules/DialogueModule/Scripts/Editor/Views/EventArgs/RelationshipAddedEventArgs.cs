namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class RelationshipAddedEventArgs
    {
        public BaseNodeView InputNodeView { get; private set; }
        public BaseNodeView OutputNodeView { get; private set; }

        public RelationshipAddedEventArgs(BaseNodeView inputNodeView, BaseNodeView outputNodeView)
        {
            InputNodeView = inputNodeView;
            OutputNodeView = outputNodeView;
        }
    }
}