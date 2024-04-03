using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Models
{
    public class BaseErrorData
    {
        public Color Color { get; set; }

        public BaseErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32(
                (byte)Random.Range(65, 256),
                (byte)Random.Range(50, 176),
                (byte)Random.Range(50, 176),
                255
            );
        }
    }
}