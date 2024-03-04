using System.Collections.Generic;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class NodeErrorData : BaseErrorData
    {
        public List<BaseNode> Nodes { get; set; }

        public NodeErrorData() : base()
        {
            Nodes = new List<BaseNode>();
        }
    }
}