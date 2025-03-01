using SDRGames.Whist.FloatingTextModule.Managers;
using SDRGames.Whist.FloatingTextModule.Views;
using SDRGames.Whist.PointsModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class CharacterCombatUIView : MonoBehaviour
    {
        [field: SerializeField] public PointsBarView ArmorPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView BarrierPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView HealthPointsBarView { get; protected set; }
        [field: SerializeField] public GridLayoutGroup EffectsBar { get; protected set; }

        [SerializeField] private VerticalLayoutGroup _floatingTextPanel;
        [SerializeField] private Vector2 _positionOffset;

        public void Initialize(Transform owner)
{
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(owner.position);
            _floatingTextPanel.transform.position = screenPosition + _positionOffset;
        }

        public void ShowFloatingText(string text, Color textColor)
        {
            FloatingTextView floatingTextView = FloatingTextManager.GetFloatingTextView();
            floatingTextView.Initialize(text, textColor);
            floatingTextView.transform.parent = _floatingTextPanel.transform;
            floatingTextView.Show();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(HealthPointsBarView), HealthPointsBarView);
            this.CheckFieldValueIsNotNull(nameof(ArmorPointsBarView), ArmorPointsBarView);
            this.CheckFieldValueIsNotNull(nameof(BarrierPointsBarView), BarrierPointsBarView);
            this.CheckFieldValueIsNotNull(nameof(EffectsBar), EffectsBar);
            this.CheckFieldValueIsNotNull(nameof(_floatingTextPanel), _floatingTextPanel);
        }
    }
}
