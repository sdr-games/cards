using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule
{
    [Serializable]
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; private set; }
        [field: SerializeField] public List<TalamusNodeView> TalamusNodes { get; private set; }
        [field: SerializeField] public List<AstraNodeView> AstraNodes { get; private set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            TalamusNodes = new List<TalamusNodeView>();
            AstraNodes = new List<AstraNodeView>();
        }

        public void AddTalamusNode(TalamusNodeView talamusNodeView)
        {
            if(!TalamusNodes.Contains(talamusNodeView))
            {
                TalamusNodes.Add(talamusNodeView);
            }
        }

        public void AddAstrahNode(AstraNodeView astraNodeView)
        {
            if(!AstraNodes.Contains(astraNodeView))
            {
                AstraNodes.Add(astraNodeView);
            }
        }
    }
}