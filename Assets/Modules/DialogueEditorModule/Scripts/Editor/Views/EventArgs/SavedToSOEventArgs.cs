using System;

using SDRGames.Whist.DialogueModule.ScriptableObjects;

namespace SDRGames.Whist.DialogueEditorModule.Views
{
    public class SavedToSOEventArgs<T> : EventArgs where T : DialogueScriptableObject
    {
        public T DialogueSO { get; private set; }

        public SavedToSOEventArgs(T dialogueSO) 
        {
            DialogueSO = dialogueSO;
        }
    }
}