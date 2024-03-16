using UnityEngine.Localization;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterForDialogues", menuName = "SDRGames/Dialogues/Character")]
    public class DialogueCharacterScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Sprite CharacterPortrait { get; private set; }
        [field: SerializeField] public LocalizedString CharacterNameLocalization { get; private set; }
    }
}
