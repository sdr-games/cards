using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class ValueFieldValueChangedEventArgs
    {
        public ScriptableObject Value { get; private set; }

        public ValueFieldValueChangedEventArgs(ScriptableObject value)
        {
            Value = value;
        }
    }
}