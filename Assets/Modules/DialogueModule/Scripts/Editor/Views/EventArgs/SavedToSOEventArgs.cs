using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
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