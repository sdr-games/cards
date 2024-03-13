using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
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
