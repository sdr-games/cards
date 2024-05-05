using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalentsBranchScriptableObject : ScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string FileName { get; set; }
        [field: SerializeField][field: ReadOnly] public Sprite Background { get; set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Talents { get; set; }
        public List<TalentScriptableObject> StartTalents => Talents.Where(x => x.Blockers.Count == 0).ToList();
        [field: SerializeField][field: ReadOnly] public List<BonusScriptableObject> Bonuses { get; set; }

        public void Initialize(string fileName, Sprite background)
        {
            FileName = fileName;
            Background = background;
            Talents = new List<TalentScriptableObject>();
            Bonuses = new List<BonusScriptableObject>();
        }
    }
}
