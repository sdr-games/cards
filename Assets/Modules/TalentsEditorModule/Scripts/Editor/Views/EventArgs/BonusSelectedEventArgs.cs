using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class BonusSelectedEventArgs
    {
        public BonusDetailWindow DetailWindow { get; private set; }

        public BonusSelectedEventArgs(BonusDetailWindow detailView)
        {
            DetailWindow = detailView;
        }
    }
}