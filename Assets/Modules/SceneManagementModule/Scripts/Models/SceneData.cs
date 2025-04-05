using System;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.MusicModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.SceneManagementModule.Models
{
    [Serializable]
    public class SceneData
    {
        [field: Header("LOADING SCREEN")][field: SerializeField] public Sprite LoadingScreenBackgroundSprite { get; private set; }
        [field: SerializeField] public MusicClipScriptableObject LoadingScreenMusic { get; private set; }
        [field: SerializeField] public LocalizedString LoadingScreenBackgroundHeader { get; private set; }
        [field: SerializeField] public LocalizedString LoadingScreenBackgroundTooltip { get; private set; }
        [field: Header("SCENE")] public string SceneName { get; private set; }
        [field: SerializeField] public SerializableDictionary<string, LocalizedString> StringParameters { get; private set; }
        [field: SerializeField] public SerializableDictionary<string, float> NumericParameters { get; private set; }
        [field: SerializeField] public SerializableDictionary<string, UnityEngine.Object> ReferenceParameters { get; private set; }

        public void SetName(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
