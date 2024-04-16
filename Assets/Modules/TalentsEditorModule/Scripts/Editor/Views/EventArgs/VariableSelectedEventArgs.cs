using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class VariableSelectedEventArgs
    {
        public VariableDetailWindow DetailWindow { get; private set; }

        public VariableSelectedEventArgs(VariableDetailWindow detailView)
        {
            DetailWindow = detailView;
        }
    }
}