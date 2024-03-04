using SDRGames.Whist.DialogueSystem;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

public class TriggerBehaviorDialogueController : MonoBehaviour
{
    [SerializeField] protected DialogueContainerScriptableObject _dialogueContainer;
    [SerializeField] protected int _selectedDialogueGroupIndex;
    [SerializeField] protected int _selectedDialogueIndex;

    [SerializeField] protected DialogueGroupScriptableObject dialogueGroup;
    [SerializeField] protected DialogueScriptableObject dialogue;

    [SerializeField] protected bool repeatable;

    // Start is called before the first frame update
    protected void Initialize()
    {
        _dialogueContainer = Resources.Load("ScriptableObjectsAssets/Dialogues/Triggers/Triggers", typeof(DialogueContainerScriptableObject)) as DialogueContainerScriptableObject;
        //_player = GameData.GetPlayer();
    }

    protected void Trigger()
    {

    }
}
