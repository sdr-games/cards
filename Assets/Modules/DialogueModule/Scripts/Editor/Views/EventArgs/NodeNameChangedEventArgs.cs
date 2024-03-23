namespace SDRGames.Whist.DialogueModule.Editor.Views
{
    public class NodeNameChangedEventArgs
    {
        public string OldNodeName { get; private set; }
        public BaseNodeView NewNode { get; private set; }

        public NodeNameChangedEventArgs(string oldNodeName, BaseNodeView newNode)
        {
            OldNodeName = oldNodeName;
            NewNode = newNode;
        }
    }
}