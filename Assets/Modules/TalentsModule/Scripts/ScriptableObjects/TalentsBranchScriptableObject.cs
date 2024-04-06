using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalentsBranchScriptableObject : ScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string FileName { get; set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Talents { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Talents = new List<TalentScriptableObject>();
        }
    }
}
