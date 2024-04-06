using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Views;

namespace SDRGames.Whist.TalentsEditorModule.Models
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