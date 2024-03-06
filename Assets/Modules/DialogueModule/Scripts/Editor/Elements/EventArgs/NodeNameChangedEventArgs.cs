namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class NodeNameChangedEventArgs
    {
        public string OldNodeName { get; private set; }
        public BaseNode NewNode { get; private set; }

        public NodeNameChangedEventArgs(string oldNodeName, BaseNode newNode)
        {
            OldNodeName = oldNodeName;
            NewNode = newNode;
        }
    }
}