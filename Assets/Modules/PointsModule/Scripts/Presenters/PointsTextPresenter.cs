using SDRGames.Islands.PointsModule.Models;
using SDRGames.Islands.PointsModule.Views;

namespace SDRGames.Islands.PointsModule.Presenters
{
    public class PointsTextPresenter
    {
        private Points _points;
        private PointsTextView _pointsView;

        public PointsTextPresenter(Points points, PointsTextView pointsView)
        {
            _points = points;
            _pointsView = pointsView;

            _points.CalculateValues();
            _pointsView.Initialize(points.MaxValue);

            _points.CurrentValueChanged += OnPointsCurrentValueChanged;
        }

        private void OnPointsCurrentValueChanged(object sender, CurrentValueChangedEventArgs e)
        {
            _pointsView.SetPointsText(e.CurrentValue);
        }
    }
}
