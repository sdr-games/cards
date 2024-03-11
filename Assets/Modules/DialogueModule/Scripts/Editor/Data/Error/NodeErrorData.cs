using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Views;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class NodeErrorData : BaseErrorData
    {
        public List<BaseNodeView> Nodes { get; set; }

        public NodeErrorData() : base()
        {
            Nodes = new List<BaseNodeView>();
        }
    }
}