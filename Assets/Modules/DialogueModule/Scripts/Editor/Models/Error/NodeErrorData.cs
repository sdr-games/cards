using System.Collections.Generic;

using SDRGames.Whist.DialogueModule.Editor.Views;

namespace SDRGames.Whist.DialogueModule.Editor.Models
{
    public class NodeErrorData : BaseErrorData
    {
        public List<BaseNodeView> Nodes { get; private set; }

        public NodeErrorData() : base()
        {
            Nodes = new List<BaseNodeView>();
        }
    }
}