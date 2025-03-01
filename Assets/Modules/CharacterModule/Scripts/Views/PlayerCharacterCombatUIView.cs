using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.PointsModule.Views;
using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class PlayerCharacterCombatUIView : CharacterCombatUIView
    {
        [field: SerializeField] public PointsBarView StaminaPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView BreathPointsBarView { get; protected set; }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(StaminaPointsBarView), StaminaPointsBarView);
            this.CheckFieldValueIsNotNull(nameof(BreathPointsBarView), BreathPointsBarView);
        }
    }
}
