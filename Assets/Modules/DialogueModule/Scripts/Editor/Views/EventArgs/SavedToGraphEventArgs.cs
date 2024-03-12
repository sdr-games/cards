using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class SavedToGraphEventArgs : EventArgs
    {
        public GraphSaveDataScriptableObject GraphData { get; private set; }
        public Vector2 Position { get; private set; }

        public SavedToGraphEventArgs(GraphSaveDataScriptableObject graphData, Vector2 position)
        {
            GraphData = graphData;
            Position = position;
        }
    }
}