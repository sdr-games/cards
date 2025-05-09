using SDRGames.Whist.FloatingTextModule.Managers;
using SDRGames.Whist.FloatingTextModule.Views;
using SDRGames.Whist.PointsModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CharacterCombatModule.Views
{
    public class CharacterCombatUIView : MonoBehaviour
    {
        [field: SerializeField] public PointsBarView ArmorPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView BarrierPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView HealthPointsBarView { get; protected set; }
        [field: SerializeField] public GridLayoutGroup EffectsBar { get; protected set; }

        [SerializeField] private VerticalLayoutGroup _floatingTextPanel;
        [SerializeField] private float _positionOffsetY;

        public void Initialize(Transform owner)
        {
            if(transform.parent == null)
            {
                return;
            } 
            Vector3 positionWithOffset = new Vector3(owner.position.x, owner.position.y + _positionOffsetY, owner.position.z);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(positionWithOffset);
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent.transform, screenPosition, null, out canvasPosition);
            ((RectTransform)_floatingTextPanel.transform).localPosition = canvasPosition;
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
