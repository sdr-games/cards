using System.Collections.Generic;

using SDRGames.Whist.DialogueEditorModule.Views;

namespace SDRGames.Whist.DialogueEditorModule.Models
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