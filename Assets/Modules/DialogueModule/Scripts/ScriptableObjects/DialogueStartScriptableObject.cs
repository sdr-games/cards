using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueStartScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public DialogueSpeechScriptableObject NextSpeech { get; set; }

        public void SetNextSpech(DialogueSpeechScriptableObject nextSpeech)
        {
            NextSpeech = nextSpeech;
        }
    }
}
