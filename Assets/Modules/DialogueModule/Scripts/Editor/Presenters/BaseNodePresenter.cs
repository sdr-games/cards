using System;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
{
    public abstract class BaseNodePresenter
    {
        public BaseNodePresenter() { }

        public abstract BaseNodeView GetNodeView();
        public abstract BaseData GetData();

        public abstract void Initialize(string name, Vector2 position);

        protected abstract void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e);

        public virtual void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            GetData().SetPosition(GetNodeView().GetPosition().position);
            //graphData.StartNodes.Add(SaveData);
        }
    }
}
