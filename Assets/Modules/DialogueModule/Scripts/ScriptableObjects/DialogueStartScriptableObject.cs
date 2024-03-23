using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueStartScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public DialogueSpeechScriptableObject NextSpeech { get; private set; }

        public void SetNextSpech(DialogueSpeechScriptableObject nextSpeech)
        {
            NextSpeech = nextSpeech;
        }
    }
}
