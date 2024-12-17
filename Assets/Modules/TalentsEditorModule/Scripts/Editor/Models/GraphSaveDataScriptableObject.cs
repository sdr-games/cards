using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule
{
    [Serializable]
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; private set; }
        [field: SerializeField] public Sprite Background { get; private set; }
        [field: SerializeField] public List<TalamusNodeView> TalamusNodes { get; private set; }
        [field: SerializeField] public List<AstraNodeView> AstraNodes { get; private set; }
        [field: SerializeField] public List<BonusData> Variables { get; private set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            TalamusNodes = new List<TalamusNodeView>();
            AstraNodes = new List<AstraNodeView>();
            Variables = new List<BonusData>();
        }

        public void SetBackgroundImage(Sprite backgroundImage)
        {
            Background = backgroundImage;
        }

        public void AddTalamusNode(TalamusNodeView talamusNodeView)
        {
            if(!TalamusNodes.Contains(talamusNodeView))
            {
                TalamusNodes.Add(talamusNodeView);
            }
        }

        public void AddAstraNode(AstraNodeView astraNodeView)
        {
            if(!AstraNodes.Contains(astraNodeView))
            {
                AstraNodes.Add(astraNodeView);
            }
        }

        public void AddVariable(BonusData variable)
        {
            if (!Variables.Contains(variable))
            {
                Variables.Add(variable);
            } 
        }
         
    }
}