using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class SavedToSOEventArgs<T> : EventArgs where T : TalentScriptableObject
    {
        public T TalentSO { get; private set; }

        public SavedToSOEventArgs(T talentSO) 
        {
            TalentSO = talentSO;
        }
    }
}