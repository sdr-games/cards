using System.Collections.Generic;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GroupErrorData : BaseErrorData
    {
        public List<Group> Groups { get; set; }

        public GroupErrorData() : base()
        {
            Groups = new List<Group>();
        }
    }
}