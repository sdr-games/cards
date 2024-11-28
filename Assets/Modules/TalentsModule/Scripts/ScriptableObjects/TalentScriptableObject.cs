using System;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    [Serializable]
    public class TalentScriptableObject : ScriptableObject
    {
        public enum NodeTypes { Talamus = 0, Astra = 1 };
        [field: SerializeField][field: ReadOnly] public string Name { get; private set; }
        [field: SerializeField][field: ReadOnly] public int Cost { get; private set; }
        [field: SerializeField] public LocalizationData DescriptionLocalization { get; private set; }
        [field: SerializeField][field: ReadOnly] public NodeTypes TalentType { get; private set; }
        [field: SerializeField][field: ReadOnly] public Vector2 PositionPercentages { get; private set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Blockers { get; private set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Dependencies { get; private set; }

        public virtual void Initialize(string name, int cost, LocalizationData descriptionLocalization, NodeTypes talentType)
        {
            Name = name;
            Cost = cost;
            DescriptionLocalization = descriptionLocalization;
            TalentType = talentType;
            Blockers = new List<TalentScriptableObject>();
            Dependencies = new List<TalentScriptableObject>();
        }

        public void SetPositionPercentages(Vector2 positionPercentages)
        {
            PositionPercentages = positionPercentages;
        }

        public Vector2 CalculatePositionInContainer()
        {
            Vector2 containerSize = new Vector2(Screen.width, Screen.height);
            return new Vector2(
                containerSize.x * PositionPercentages.x / 100,
                containerSize.y - containerSize.y * PositionPercentages.y / 100
            );
        }
    }
}
